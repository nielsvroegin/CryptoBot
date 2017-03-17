using System;
using System.Collections.Concurrent;
using CryptoBot.Utils.General;
using CryptoBot.Utils.Assertions;

namespace CryptoBot.Instrument
{
    public sealed class InstrumentManager
    {
        private readonly ConcurrentDictionary<String, LiveInstrument> _liveInstruments = new ConcurrentDictionary<String, LiveInstrument>();

        public void Update(Exchange exchange, TickData tickData)
        {
            Preconditions.CheckNotNull(exchange);
            Preconditions.CheckNotNull(tickData);

            var currencyPair = tickData.CurrencyPair;
            var instrumentKey = string.Format("{0}_{1}_{2}", exchange, currencyPair.BaseCurrency, currencyPair.QuoteCurrency);

            // Get or add the instrument
            var instrument  = _liveInstruments.GetOrAdd(instrumentKey, s => new LiveInstrument(exchange, currencyPair));

            // Update instrument with TickData
            instrument.Update(tickData);
        }
    }
}
