using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoBot.ExchangeApi.Market;
using CryptoBot.Instrument.Live;
using CryptoBot.Instrument.Static;
using CryptoBot.Utils.General;
using CryptoBot.Utils.Assertions;

namespace CryptoBot.Instrument
{
    public sealed class InstrumentManager : IInstrumentManager
    {
        private readonly IList<LiveOhlcInstrument> _liveInstruments = new List<LiveOhlcInstrument>();

        public void Create(Exchange exchange, CurrencyPair currencyPair, IMarketApi marketApi)
        {
            Preconditions.CheckNotNull(exchange);
            Preconditions.CheckNotNull(currencyPair);
            Preconditions.CheckNotNull(marketApi);

            // Check if LiveInstrument doesn't already exist for exchange/currenypair
            if (_liveInstruments.Any(i => i.Exchange == exchange && i.CurrencyPair.Equals(currencyPair)))
            {
                return;
            }

            _liveInstruments.Add(new LiveOhlcInstrument(exchange, currencyPair, marketApi));
        }

        public async Task<OhlcInstrument> Update(TickData tickData)
        {
            Preconditions.CheckNotNull(tickData);

            var instrument = _liveInstruments.FirstOrDefault(i => i.Exchange == tickData.Exchange && i.CurrencyPair.Equals(tickData.CurrencyPair));

            // When instrument doesn't exist, no update necessary
            if (ReferenceEquals(instrument, null))
            {
                return null;
            }
            
            // Update instrument with TickData
            return await instrument.Update(tickData);
        }
    }
}
