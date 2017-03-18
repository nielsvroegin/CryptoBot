using System.Threading.Tasks;
using CryptoBot.ExchangeApi.Market;
using CryptoBot.Instrument.Static;
using CryptoBot.Utils.General;

namespace CryptoBot.Instrument
{
    public interface IInstrumentManager
    {
        void Create(Exchange exchange, CurrencyPair currencyPair, IMarketApi marketApi);
        Task<OhlcInstrument> Update(TickData tickData);
    }
}