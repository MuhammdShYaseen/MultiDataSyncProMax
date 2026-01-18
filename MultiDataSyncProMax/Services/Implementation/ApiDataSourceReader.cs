using MultiDataSyncProMax.Configuration.ProfilesModels;
using MultiDataSyncProMax.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MultiDataSyncProMax.Services.Implementation
{
    public class ApiDataSourceReader : IDataSourceReader
    {
        private readonly HttpClient _client = new();

        public async IAsyncEnumerable<Dictionary<string, object?>> ReadAsync(SourceConfig config)
        {
            int skip = 0;
            while (true)
            {
                var url = $"{config.Endpoint}?limit={config.PageSize}&skip={skip}";
                var json = await _client.GetStringAsync(url);
                using var doc = JsonDocument.Parse(json);

                if (!doc.RootElement.TryGetProperty(config.RootArray!, out var array))
                    yield break;

                if (array.GetArrayLength() == 0)
                    yield break;

                foreach (var element in array.EnumerateArray())
                {
                    var record = new Dictionary<string, object?>();
                    foreach (var map in config.Fields)
                    {
                        if (element.TryGetProperty(map.Value, out var prop))
                            record[map.Key] = prop.ToString();
                        Console.WriteLine($"Key {map.Key} value {map.Value}");
                        
                    }
                    yield return record;
                }

                skip += config.PageSize;
            }
        }
    }
}
