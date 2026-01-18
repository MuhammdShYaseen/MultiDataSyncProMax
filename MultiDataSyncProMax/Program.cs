using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MultiDataSyncProMax.Extensions;
using MultiDataSyncProMax.Services;

namespace MultiDataSyncProMax
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args).ConfigureServices(services => {
                services.AddDataSync();
            }).Build();

            await host.Services.GetRequiredService<AppRunner>().RunAsync();
        }
    }
}
