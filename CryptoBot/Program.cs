using System;
using System.Runtime.Loader;
using System.Threading;
using CryptoBot.Bots;
using CryptoBot.Bots.Strategies.Cowabunga;
using CryptoBot.TickerServices.Services.Poloniex;
using CryptoBot.Utils.Logging;
using CryptoBot.Utils.ServiceHandler;
using Microsoft.Extensions.Logging;

namespace CryptoBot
{
    internal class Program
    {
        private static readonly ILogger Logger = ApplicationLogging.CreateLogger<Program>();
        private static readonly ManualResetEvent QuitEvent = new ManualResetEvent(false);
        private static readonly ServiceHandler ServiceHandler = new ServiceHandler();

        // ReSharper disable once UnusedMember.Local
        private static void Main()
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
            ApplicationLogging.LoggerFactory.AddConsole(LogLevel.Debug, true);
        }

        private static void StartApp()
        {
            Logger.LogInformation("CryptoBot starting...");
            
            // Create ticker services
            var poloniexTickerService = ServiceHandler.Start(new PoloniexTickerService());

            // Create bot strategies
            var cowabungaStrategy = new CowabungaStrategy();

            // Build and start BotHandlerService
            var botHandlerService = new BotHandlerService.Builder()
                .RegisterTickerService(poloniexTickerService)
                .RegisterBotStrategy(cowabungaStrategy)
                .Build();

            ServiceHandler.Start(botHandlerService);
        }

        private static void StopApp()
        {
            // Stop all started services
            ServiceHandler.StopAll();

            Logger.LogInformation("CryptoBot stopped");
        }
    }
}
