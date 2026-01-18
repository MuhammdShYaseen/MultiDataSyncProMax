using MultiDataSyncProMax.Configuration.ProfilesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiDataSyncProMax.Configuration.Interfaces
{
    public interface IProfileLoader
    {
        SyncProfile Load(string path);
    }
}
