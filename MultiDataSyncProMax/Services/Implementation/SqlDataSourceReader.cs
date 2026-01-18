using Microsoft.Data.SqlClient;
using MultiDataSyncProMax.Configuration.ProfilesModels;
using MultiDataSyncProMax.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiDataSyncProMax.Services.Implementation
{
    public class SqlDataSourceReader : IDataSourceReader
    {
        public async IAsyncEnumerable<Dictionary<string, object?>> ReadAsync(SourceConfig config)
        {
            await using var conn = new SqlConnection(config.ConnectionString);
            await conn.OpenAsync();

            var columns = string.Join(", ", config.Fields.Values.Select(c => $"[{c}]"));
            var cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT {columns} FROM [{config.Table}]";

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var record = new Dictionary<string, object?>();
                foreach (var map in config.Fields)
                {
                    var value = reader[map.Value];
                    record[map.Key] = value == DBNull.Value ? null : value;
                }
                yield return record;
            }
        }
    }
}
