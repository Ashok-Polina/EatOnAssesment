
namespace FuelConsumptionCentralMonitoringSystem.Helpers
{
    using System;
    /// <summary>
    /// Helper Method to log Messages to console
    /// </summary>
        public class Logger
        {
            public static void Log(string text, ConsoleColor color = ConsoleColor.White)
            {
                // since this method might be called by different threads,
                // we need to use a lock to guarantee we set the right color
                lock (Console.Out)
                {
                    Console.ForegroundColor = color;
                    Console.WriteLine($"[{DateTime.UtcNow:hh:mm:ss.ff}] - {text}");
                }
            }
        }
    
}
