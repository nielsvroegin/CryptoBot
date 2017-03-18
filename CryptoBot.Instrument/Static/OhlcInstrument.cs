using System.Collections.Generic;
using System.Collections.Immutable;
using CryptoBot.Utils.Assertions;
using CryptoBot.Utils.General;

namespace CryptoBot.Instrument.Static
{
    public class OhlcInstrument
    {
        public Exchange Exchange { get; }
        public CurrencyPair CurrencyPair { get; }
        public ImmutableList<OhlcSeries> OhlcSeries { get; }

        public OhlcInstrument(Exchange exchange, CurrencyPair currencyPair, IList<OhlcSeries> ohlcSeries)
        {
            Preconditions.CheckNotNull(ohlcSeries);

            Exchange = Preconditions.CheckNotNull(exchange);
            CurrencyPair = Preconditions.CheckNotNull(currencyPair);
            OhlcSeries = ImmutableList.CreateRange(ohlcSeries);
        }
    }
}
