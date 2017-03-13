using System;
using System.Runtime.Loader;
using System.Threading;
using CryptoBot.TickerServices.Services.Poloniex;
using CryptoBot.Utils.Logging;
using Microsoft.Extensions.Logging;

namespace CryptoBot
{
    internal class Program
    {
        private static readonly ILogger Logger = ApplicationLogging.CreateLogger<Program>();
        private static readonly ManualResetEvent QuitEvent = new ManualResetEvent(false);

        private static void Main(string[] args)
        {
            // Configure logging providers
            ConfigureLogging();
            
            // Configure quit events
            Console.CancelKeyPress += (sender, eArgs) => {
                QuitEvent.Set();
                eArgs.Cancel = true;
            };
            AssemblyLoadContext.Default.Unloading += context =>
            {
                QuitEvent.Set();
            };

            // Start the application
            StartApp();

            // Wait for quit event
            QuitEvent.WaitOne();

            // Stop the application
            StopApp();
        }

        private static void ConfigureLogging()
        {
            ApplicationLogging.LoggerFactory.AddConsole(LogLevel.Trace, true);
        }

        private static void StartApp()
        {
            Logger.LogInformation("CryptoBot starting...");

            var service = new PoloniexTickerService();
            service.Start();
        }

        private static void StopApp()
        {


            Logger.LogInformation("CryptoBot stopped");
        }
    }
}
