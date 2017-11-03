using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoBot.ExchangeApi.Market;
using CryptoBot.ExchangeMonitors;
using CryptoBot.Instrument;
using CryptoBot.Utils.General;
using CryptoBot.Utils.Logging;
using CryptoBot.Utils.Assertions;
using CryptoBot.Utils.ServiceHandler;
using log4net;

namespace CryptoBot.Bots
{
    /// <summary>
    /// Service that runs the registered strategies and provides information to these services
    /// </summary>
    public sealed class BotHandlerService : IManagedService, IExchangeMonitorSubscriber
    {
        private static readonly ILog Logger = ApplicationLogging.CreateLogger<BotHandlerService>();

        private readonly IInstrumentManager _instrumentManager;
        private readonly IList<IBotStrategy> _botStrategies = new List<IBotStrategy>();
        private readonly IList<IExchangeMonitor> _exchangeMonitors = new List<IExchangeMonitor>();
        private readonly IList<IMarketApi> _marketApis = new List<IMarketApi>();

        /// <summary>
        /// Constructor, use builder for creation
        /// </summary>
        private BotHandlerService(IInstrumentManager instrumentManager)
        {
            _instrumentManager = Preconditions.CheckNotNull(instrumentManager);
        }

        /// <summary>
        /// Start the bot handler
        /// </summary>
        /// <exception cref="BotHandlerException">Exception thrown when BotHandler could not be started</exception>
        public void Start()
        {
            // Verify al dependencies registered for bot strategies and init the startegy
            foreach (var botStrategy in _botStrategies)
            {
                // Verify an exchange monitor for this exchange has been registered
                if (_exchangeMonitors.All(t => t.Exchange != botStrategy.Exchange))
                {
                    throw new BotHandlerException($"No ticker service registered for exchange '{botStrategy.Exchange}', which is used by BotStrategy '{nameof(botStrategy)}'.");
                }

                // Verify a market api for this exchange has been registered
                if (_marketApis.All(m => m.Exchange != botStrategy.Exchange))
                {
                    throw new BotHandlerException($"No market api registered for exchange '{botStrategy.Exchange}', which is used by BotStrategy '{nameof(botStrategy)}'.");
                }

                var marketApi = _marketApis.First(m => m.Exchange == botStrategy.Exchange);

                // Create instruments
                _instrumentManager.Create(botStrategy.Exchange, botStrategy.CurrencyPair, marketApi);

                // Init the strategy
                botStrategy.Init(marketApi);
            }

            // Subscribe on exchange monitors
            foreach (var exchangeMonitor in _exchangeMonitors)
            {
                foreach (var currencyPair in _botStrategies.Select(b => b.CurrencyPair).Distinct())
                {
                    exchangeMonitor.Subscribe(currencyPair, this);
                }
            }

            Logger.Info("BotHandlerService has been started");
        }

        /// <inheritdoc />
        public void Stop()
        {
            // Deinit strategies
            foreach (var botStrategy in _botStrategies)
            {
                botStrategy.Deinit();
            }

            // Unsubscribe from exchange monitors
            foreach (var exchangeMonitor in _exchangeMonitors)
            {
                foreach (var currencyPair in _botStrategies.Select(b => b.CurrencyPair).Distinct())
                {
                    exchangeMonitor.Unsubscribe(currencyPair, this);
                }
            }

            Logger.Info("BotHandlerService has been stopped");
        }

        /// <summary>
        /// Tick method called on new tick of a TickerService
        /// </summary>
        /// <param name="exchange">Exchange that provided this tick</param>
        /// <param name="tickData">Information regarding tick</param>
        public void OnTick(Exchange exchange, TickData tickData)
        {
            Preconditions.CheckNotNull(exchange);
            Preconditions.CheckNotNull(tickData);

            Task.Run(() => ProcessTick(exchange, tickData));
        }

        private async Task ProcessTick(Exchange exchange, TickData tickData)
        {
            try
            {
                // Update instruments
                var instrument = await _instrumentManager.Update(tickData);

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

                    // Send tick and other relevant information to strategy
                    var marketApi = _marketApis.First(m => m.Exchange == botStrategy.Exchange);
                    botStrategy.HandleTick(tickData, instrument, marketApi);
                }
            }
            catch (Exception e)
            {
                Logger.Error("Uncaught exception during tick processing.", e);
            }
        }

        /// <summary>
        /// Builder for BotHandlerService
        /// </summary>
        public sealed class Builder
        {
            private BotHandlerService _botHandlerService;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="instrumentManager">InstrumentManager depency</param>
            public Builder(IInstrumentManager instrumentManager)
            {
                _botHandlerService = new BotHandlerService(instrumentManager);
            }

            /// <summary>
            /// Register an exchange monitor to use in BotHandlerService
            /// </summary>
            public Builder RegisterExchangeMonitor(IExchangeMonitor exchangeMonitor)
            {
                Preconditions.CheckNotNull(exchangeMonitor);

                _botHandlerService._exchangeMonitors.Add(exchangeMonitor);
                return this;
            }

            /// <summary>
            /// Register a market api to use in BotHandlerService
            /// </summary>
            /// <param name="marketApi"></param>
            /// <returns></returns>
            public Builder RegisterMarketApi(IMarketApi marketApi)
            {
                Preconditions.CheckNotNull(marketApi);

                _botHandlerService._marketApis.Add(marketApi);
                return this;
            }

            /// <summary>
            /// Register a ticker service to use in BotHandlerService
            /// </summary>
            public Builder RegisterBotStrategy(IBotStrategy botStrategy)
            {
                Preconditions.CheckNotNull(botStrategy);

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
