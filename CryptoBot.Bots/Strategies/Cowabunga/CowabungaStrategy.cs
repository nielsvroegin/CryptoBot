using CryptoBot.ExchangeApi.Market;
using CryptoBot.Instrument.Static;
using CryptoBot.Utils.General;
using CryptoBot.Utils.Logging;
using log4net;

namespace CryptoBot.Bots.Strategies.Cowabunga
{
    /// <summary>
    /// The Cowabunga bot strategy
    /// </summary>
    public class CowabungaStrategy : IBotStrategy
    {
        private static readonly ILog Logger = ApplicationLogging.CreateLogger<CowabungaStrategy>();

        /// <inheritdoc />
        public Exchange Exchange => Exchange.Poloniex;

        /// <inheritdoc />
        public CurrencyPair CurrencyPair => new CurrencyPair(Currency.Btc, Currency.Eth);

        /// <inheritdoc />
        public void Init(IMarketApi marketApi)
        {
            Logger.Info("Cowabunga strategy initialized");
        }

        /// <inheritdoc />
        public void Deinit()
        {
            Logger.Info("Cowabunga strategy deinitialized");
        }

        /// <inheritdoc />
        public void HandleTick(TickData tickData, OhlcInstrument instrument, IMarketApi marketApi)
        {
            double ema = instrument.OhlcSeries.Find(s => s.TimeSpanSeconds == 900).Ema(9);
            Logger.Info($"Cowabunga ema: {ema}");
        }
    }
}
