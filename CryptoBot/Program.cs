using System;
using System.Threading;
using CryptoBot.Bots;
using CryptoBot.Bots.Strategies.Cowabunga;
using CryptoBot.ExchangeApi.Market.Poloniex;
using CryptoBot.Instrument;
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
            
            // TODO: Handle linux signals

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

            // Create market apis
            var poloniexMarketApi = new PoloniexMarketApi();

            // Create bot strategies
            var cowabungaStrategy = new CowabungaStrategy();

            // Build BotHandlerService
            var botHandlerService = new BotHandlerService.Builder(new InstrumentManager())
                .RegisterTickerService(poloniexTickerService)
                .RegisterMarketApi(poloniexMarketApi)
                .RegisterBotStrategy(cowabungaStrategy)
                .Build();

            // Start the BotHandlerService
            try
            {
                ServiceHandler.Start(botHandlerService);
            }
            catch (BotHandlerException e)
            {
                Logger.LogCritical("BotHandler could not be started", e);
            }
        }

        private static void StopApp()
        {
            // Stop all started services
            ServiceHandler.StopAll();

            Logger.LogInformation("CryptoBot stopped");
        }
    }
}
