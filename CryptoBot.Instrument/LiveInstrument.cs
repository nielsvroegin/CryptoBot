using System;
using System.Collections.Generic;
using System.Text;
using CryptoBot.Instrument.Ohlc;
using CryptoBot.Utils.General;
using CryptoBot.Utils.Preconditions;

namespace CryptoBot.Instrument
{
    internal class LiveInstrument
    {
        private readonly object _updateLock = new object();
        private readonly Exchange _exchange;
        private readonly CurrencyPair _currencyPair;

        private readonly LiveOhlcSeries _minuteSeries = new LiveOhlcSeries(300);

        public LiveInstrument(Exchange exchange, CurrencyPair currencyPair)
        {
            _exchange = Preconditions.CheckNotNull(exchange); ;
            _currencyPair = Preconditions.CheckNotNull(currencyPair); ;
        }

        internal void Update(TickData tickData)
        {
            lock (_updateLock)
            {
                _minuteSeries.AddTick(tickData);
            }
        }
    }
}
