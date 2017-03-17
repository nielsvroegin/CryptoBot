using System.Collections.Generic;
using CryptoBot.Instrument.Ohlc;
using CryptoBot.Utils.General;
using CryptoBot.Utils.Assertions;

namespace CryptoBot.Instrument
{
    internal class LiveInstrument
    {
        private readonly object _updateLock = new object();
        private readonly Exchange _exchange;
        private readonly CurrencyPair _currencyPair;
        private readonly IList<LiveOhlcSeries> _ohlcSeries = new List<LiveOhlcSeries>();

        public LiveInstrument(Exchange exchange, CurrencyPair currencyPair)
        {
            _exchange = Preconditions.CheckNotNull(exchange); ;
            _currencyPair = Preconditions.CheckNotNull(currencyPair);

            // Add Ohlc series to dictionary
            _ohlcSeries.Add(new LiveOhlcSeries(300)); // 5 min
			_ohlcSeries.Add(new LiveOhlcSeries(900)); // 15 min
			_ohlcSeries.Add(new LiveOhlcSeries(1800)); // 30 min
			_ohlcSeries.Add(new LiveOhlcSeries(3600)); // 2 hour
			_ohlcSeries.Add(new LiveOhlcSeries(86400)); // 1 day
        }

        internal void Update(TickData tickData)
        {
            lock (_updateLock)
            {
                foreach (var ohlcSeries in _ohlcSeries)
                {
                    // When OhlcSeries doesn't contain any data yet, try to initialize
                    if (!ohlcSeries.HasData)
                    {
                        InitializeSeries(ohlcSeries);
                    }

                    // Provide tick to series
					ohlcSeries.AddTick(tickData);
                }
            }
        }

        private void InitializeSeries(LiveOhlcSeries ohlcSeries)
        {
            //throw new NotImplementedException();
        }
   }
}
