using System.Collections.Generic;
using CryptoBot.TickerServices;
using CryptoBot.TickerServices.Data;
using CryptoBot.Utils.Enums;
using CryptoBot.Utils.Logging;
using CryptoBot.Utils.ServiceHandler;
using Microsoft.Extensions.Logging;

namespace CryptoBot.Bots
{
    /// <summary>
    /// Service that runs the registered strategies and provides information to these services
    /// </summary>
    public sealed class BotHandlerService : IManagedService, ITickerSubscriber
    {
        private static readonly ILogger Logger = ApplicationLogging.CreateLogger<BotHandlerService>();

        private readonly IList<IBotStrategy> _botStrategies = new List<IBotStrategy>();
        private readonly IList<ITickerService> _tickerServices = new List<ITickerService>();

        /// <summary>
        /// Constructor, use builder for creation
        /// </summary>
        private BotHandlerService() { }

        /// <inheritdoc />
        public void Start()
        {
            // Init strategies
            foreach (var botStrategy in _botStrategies)
            {
                botStrategy.Init();
            }

            // Subscribe on tickers
            foreach (var tickerService in _tickerServices)
            {
                tickerService.Subscribe(this);
            }

            Logger.LogInformation("BotHandlerService has been started");
        }

        /// <inheritdoc />
        public void Stop()
        {
            // Deinit strategies
            foreach (var botStrategy in _botStrategies)
            {
                botStrategy.Deinit();
            }

            // Unsubscribe from tickers
            foreach (var tickerService in _tickerServices)
            {
                tickerService.Unsubscribe(this);
            }

            Logger.LogInformation("BotHandlerService has been stopped");
        }

        /// <summary>
        /// Tick method called on new tick of a TickerService
        /// </summary>
        /// <param name="exchange">Exchange that provided this tick</param>
        /// <param name="tickData">Information regarding tick</param>
        public void OnTick(Exchange exchange, TickData tickData)
        {
            foreach (var botStrategy in _botStrategies)
            {
                // Verify if bot strategy was created for this exchange
                if (botStrategy.Exchange != exchange)
                {
                    continue;
                }

                // Verify currency pair used by this bot strategy
                if (!botStrategy.CurrencyPair.Equals(tickData.CurrencyPair))
                {
                    continue;
                }

                botStrategy.HandleTick(tickData);
            }
        }

        /// <summary>
        /// Builder for BotHandlerService
        /// </summary>
        public sealed class Builder
        {
            private BotHandlerService _botHandlerService = new BotHandlerService();

            /// <summary>
            /// Register a ticker service to use in BotHandlerService
            /// </summary>
            public Builder RegisterTickerService(ITickerService tickerService)
            {
                _botHandlerService._tickerServices.Add(tickerService);
                return this;
            }

            /// <summary>
            /// Register a ticker service to use in BotHandlerService
            /// </summary>
            public Builder RegisterBotStrategy(IBotStrategy botStrategy)
            {
                _botHandlerService._botStrategies.Add(botStrategy);
                return this;
            }

            /// <summary>
            /// Build BotHandlerService and make sure it can't be altered by builder anymore
            /// </summary>
            /// <returns>The build BotHandlerService object</returns>
            public BotHandlerService Build()
            {
                var ret = _botHandlerService;
                _botHandlerService = null;
                return ret;
            }
        }
    }
}
