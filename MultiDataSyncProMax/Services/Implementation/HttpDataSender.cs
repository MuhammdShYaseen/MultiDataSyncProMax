using MultiDataSyncProMax.GlobalErrorHandler;
using MultiDataSyncProMax.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MultiDataSyncProMax.Services.Implementation
{
    public class HttpDataSender : IDataSender
    {
        private readonly IHttpClientFactory _clientFactory;

        public HttpDataSender(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task SendAsync(string endpoint, object payload)
        {
            await ErrorMiddleware.ExecuteWithHandling(async () =>
            {
                var client = _clientFactory.CreateClient("DataSync");
                var content = new StringContent(
                    JsonSerializer.Serialize(payload),
                    Encoding.UTF8,
                    "application/json");

                var response = await client.PostAsync(endpoint, content);
                response.EnsureSuccessStatusCode();

                Console.WriteLine($"📤 Sent successfully to {endpoint}");
            });
        }
    }
}
