using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiDataSyncProMax.Configuration.ProfilesModels
{
    public class SourceConfig
    {
        public string Type { get; set; } = "Api"; // Api | Sql

        // API
        public string? Endpoint { get; set; }
        public string? RootArray { get; set; }
        public int PageSize { get; set; } = 100;

        // SQL
        public string? ConnectionString { get; set; }
        public string? Table { get; set; }

        // Common
        public Dictionary<string, string> Fields { get; set; } = new();
    }
}
