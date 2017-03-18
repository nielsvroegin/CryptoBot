using CryptoBot.ExchangeApi.Market;
using CryptoBot.Instrument.Static;
using CryptoBot.Utils.General;
using CryptoBot.Utils.Logging;
using Microsoft.Extensions.Logging;

namespace CryptoBot.Bots.Strategies.Cowabunga
{
    /// <summary>
    /// The Cowabunga bot strategy
    /// </summary>
    public class CowabungaStrategy : IBotStrategy
    {
        private static readonly ILogger Logger = ApplicationLogging.CreateLogger<CowabungaStrategy>();

        /// <inheritdoc />
        public Exchange Exchange => Exchange.Poloniex;

        /// <inheritdoc />
        public CurrencyPair CurrencyPair => new CurrencyPair(Currency.Btc, Currency.Eth);

        /// <inheritdoc />
        public void Init(IMarketApi marketApi)
        {
            Logger.LogInformation("Cowabunga strategy initialized");
        }

        /// <inheritdoc />
        public void Deinit()
        {
            Logger.LogInformation("Cowabunga strategy deinitialized");
        }

        /// <inheritdoc />
        public void HandleTick(TickData tickData, OhlcInstrument instrument, IMarketApi marketApi)
        {
            Logger.LogInformation("Cowabunga tickdata: {0}", tickData);
        }
    }
}
