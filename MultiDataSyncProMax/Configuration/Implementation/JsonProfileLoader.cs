using Microsoft.Extensions.Configuration;
using MultiDataSyncProMax.Configuration.Interfaces;
using MultiDataSyncProMax.Configuration.ProfilesModels;

namespace MultiDataSyncProMax.Configuration.Implementation
{
    public class JsonProfileLoader : IProfileLoader
    {
        public SyncProfile Load(string path)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile(path, false)
                .Build();

            var profile = new SyncProfile();
            config.Bind(profile);
            return profile;
        }
    }
}
