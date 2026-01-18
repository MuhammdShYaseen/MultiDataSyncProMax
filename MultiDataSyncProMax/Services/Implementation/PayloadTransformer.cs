using MultiDataSyncProMax.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MultiDataSyncProMax.Services.Implementation
{
    public class PayloadTransformer : IPayloadTransformer
    {
        public object Transform(object template, Dictionary<string, object?> source)
        {
            var json = JsonSerializer.Serialize(template);
            foreach (var kv in source)
                json = json.Replace($"{{{kv.Key}}}", kv.Value?.ToString());

            return JsonSerializer.Deserialize<object>(json)!;
        }
    }
}
