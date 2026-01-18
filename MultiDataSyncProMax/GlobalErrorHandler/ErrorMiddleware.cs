

namespace MultiDataSyncProMax.GlobalErrorHandler
{
    public static class ErrorMiddleware
    {
        public static async Task<T> ExecuteWithHandling<T>(Func<Task<T>> operation)
        {
            try
            {
                return await operation();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ ERROR: {ex.Message}");
                Console.ResetColor();
                throw;
            }
        }

        public static async Task ExecuteWithHandling(Func<Task> operation)
        {
            try
            {
                await operation();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ ERROR: {ex.Message}");
                Console.ResetColor();
                throw;
            }
        }
    }
}
