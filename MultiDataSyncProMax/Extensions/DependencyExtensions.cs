using Microsoft.Extensions.DependencyInjection;
using MultiDataSyncProMax.Configuration.Implementation;
using MultiDataSyncProMax.Configuration.Interfaces;
using MultiDataSyncProMax.Services;
using MultiDataSyncProMax.Services.Implementation;
using MultiDataSyncProMax.Services.Interfaces;
using Polly;
using Polly.Extensions.Http;

namespace MultiDataSyncProMax.Extensions
{
    public static class DependencyExtensions
    {
        public static IServiceCollection AddDataSync(this IServiceCollection services)
        {
            services.AddSingleton<IProfileLoader, JsonProfileLoader>();
            services.AddSingleton<IDataSourceReaderFactory, DataSourceReaderFactory>();
            services.AddSingleton<IPayloadTransformer, PayloadTransformer>();
            services.AddSingleton<IDataSender, HttpDataSender>();
            services.AddSingleton<IStateStore, FileStateStore>();
            services.AddTransient<DataSyncService>();
            services.AddSingleton<AppRunner>();

            // Add HttpClient with retry policy
            services.AddHttpClient("DataSync").AddTransientHttpErrorPolicy(policy => 
                                                                             policy.WaitAndRetryAsync(retryCount: 3, sleepDurationProvider: retryAttempt =>
                                                                                 TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

            return services;
        }
    }
}
