using MultiDataSyncProMax.Configuration.Interfaces;
using MultiDataSyncProMax.GlobalErrorHandler;
using MultiDataSyncProMax.Services.Interfaces;

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

                var batch = new List<object>();
                var pageSize = profile.Source.PageSize > 0
                    ? profile.Source.PageSize
                    : 30;

                await foreach (var record in reader.ReadAsync(profile.Source))
                {
                    try
                    {
                        var payload = _transformer.Transform(
                            profile.Destination.PayloadTemplate,
                            record
                        );

                        batch.Add(payload);
                        processed++;
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"⚠️ Failed to transform record: {ex.Message}");
                        Console.ResetColor();
                    }
                }

                // ===============================
                // إرسال البيانات على صفحات
                // ===============================
                if (batch.Count == 0)
                {
                    Console.WriteLine("⚠️ No data to send.");
                    return;
                }

                Console.WriteLine($"📦 Sending {batch.Count} records in pages of {pageSize}");

                int pageNumber = 1;

                foreach (var page in batch.Chunk(pageSize))
                {
                    Console.WriteLine($"📤 Sending page {pageNumber} ({page.Length} records)");

                    await _sender.SendAsync(
                        profile.Destination.Endpoint,
                        page.ToList()
                    );

                    pageNumber++;

                    // تأخير بسيط لتخفيف الضغط على السيرفر
                    await Task.Delay(200);
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ Sync completed successfully!");
                Console.WriteLine($"📊 Total: {processed} processed, {skipped} skipped");
                Console.ResetColor();
            });
        }
    }
}
