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
            var client = _clientFactory.CreateClient("DataSync");

            object json = payload is string s
                ? JsonDocument.Parse(s).RootElement
                : payload;

            var jsonString = JsonSerializer.Serialize(json);

            var content = new StringContent(
                jsonString,
                Encoding.UTF8,
                "application/json");

            Console.WriteLine("Outgoing Payload:\n" + JsonSerializer.Serialize(json, new JsonSerializerOptions { WriteIndented = true }));

            var response = await client.PostAsync(endpoint, content);
            response.EnsureSuccessStatusCode();

            Console.WriteLine($"📤 Sent successfully to {endpoint}");
        }
    }
}
