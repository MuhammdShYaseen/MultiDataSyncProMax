using MultiDataSyncProMax.Configuration.ProfilesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiDataSyncProMax.Services.Interfaces
{
    public interface IDataSourceReader
    {
        IAsyncEnumerable<Dictionary<string, object?>> ReadAsync(SourceConfig config);
    }
}
