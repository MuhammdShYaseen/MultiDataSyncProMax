using MultiDataSyncProMax.Configuration.ProfilesModels;
using MultiDataSyncProMax.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiDataSyncProMax.Services.Implementation
{
    public class DataSourceReaderFactory : IDataSourceReaderFactory
    {
        public IDataSourceReader Create(SourceConfig config)
        {
            return config.Type switch
            {
                "Sql" => new SqlDataSourceReader(),
                _ => new ApiDataSourceReader()
            };
        }
    }

}
