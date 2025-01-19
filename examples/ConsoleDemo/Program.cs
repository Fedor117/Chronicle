using Chronicle;
using Chronicle.Context;
using Chronicle.Implementations;

namespace ConsoleDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var logPath = Path.Combine(Directory.GetCurrentDirectory(), "game.log");

            // Create logger
            using var logger = new FileChronicler(logPath);

            // Change minimum level if needed
            ChronicleContext.Current.MinimumLevel = ChronicleLevel.Debug;

            Console.WriteLine($"Writing logs to: {logPath}");

            // Log some game events
            logger.Info()
                .Message("Game initialization started")
                .Property("timestamp", DateTime.UtcNow)
                .Write();

            // Simulate loading a level
            logger.Debug()
                .Message("Loading Level 1")
                .Property("levelId", "level_001")
                .Write();

            // Simulate some error
            try
            {
                throw new Exception("Missing texture asset");
            }
            catch (Exception ex)
            {
                logger.Error()
                    .Message("Failed to load asset")
                    .Property("error", ex.Message)
                    .Write();
            }

            logger.Info()
                .Message("Game initialization completed")
                .Write();

            Console.WriteLine("Done! Check the log file.");
        }
    }
}