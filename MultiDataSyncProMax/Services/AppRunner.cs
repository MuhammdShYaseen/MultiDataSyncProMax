using MultiDataSyncProMax.Configuration.Interfaces;
using MultiDataSyncProMax.GlobalErrorHandler;
using MultiDataSyncProMax.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiDataSyncProMax.Services
{
    public class AppRunner
    {
        private readonly IProfileLoader _loader;
        private readonly IDataSourceReaderFactory _factory;
        private readonly IPayloadTransformer _transformer;
        private readonly IDataSender _sender;
        private readonly IStateStore _state;

        public AppRunner(
            IProfileLoader loader,
            IDataSourceReaderFactory factory,
            IPayloadTransformer transformer,
            IDataSender sender,
            IStateStore state)
        {
            _loader = loader;
            _factory = factory;
            _transformer = transformer;
            _sender = sender;
            _state = state;
        }

        public async Task RunAsync()
        {
            await ErrorMiddleware.ExecuteWithHandling(async () =>
            {
                Console.Write("JSON profile path: ");
                var path = Console.ReadLine()!.Trim();

                Console.WriteLine($"📁 Loading profile from: {path}");
                var profile = _loader.Load(path);

                Console.WriteLine($"🔄 Starting sync from {profile.Source.Type} to {profile.Destination.Endpoint}");

                var reader = _factory.Create(profile.Source);
                int processed = 0;
                int skipped = 0;

                await foreach (var record in reader.ReadAsync(profile.Source))
                {
                    if (!record.TryGetValue("Id", out var id) || id == null)
                    {
                        Console.WriteLine($"⚠️ Record without ID skipped: {System.Text.Json.JsonSerializer.Serialize(record)}");
                        skipped++;
                        continue;
                    }

                    var idString = id.ToString()!;
                    if (_state.IsProcessed(idString))
                    {
                        skipped++;
                        continue;
                    }

                    try
                    {
                        var payload = _transformer.Transform(profile.Destination.PayloadTemplate, record);
                        await _sender.SendAsync(profile.Destination.Endpoint, payload);
                        _state.MarkProcessed(idString);
                        processed++;

                        if (processed % 10 == 0)
                            Console.WriteLine($"📊 Progress: {processed} processed, {skipped} skipped");
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"⚠️ Failed to process record {idString}: {ex.Message}");
                        Console.ResetColor();
                        // Continue with next record
                    }
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ Sync completed successfully!");
                Console.WriteLine($"📊 Total: {processed} processed, {skipped} skipped");
                Console.ResetColor();
            });
        }
    }
}
