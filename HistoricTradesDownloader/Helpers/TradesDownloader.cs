using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoBot.ExchangeApi.Market;
using CryptoBot.Utils.Assertions;
using CryptoBot.Utils.General;

namespace HistoricTradesDownloader.Helpers
{
    internal class TradesDownloader
    {
        private readonly ImmutableList<IMarketApi> _marketApis;

        public TradesDownloader(IList<IMarketApi> marketApis)
        {
            _marketApis = ImmutableList.CreateRange(marketApis);
        }

        public IList<Exchange> GetExchanges()
        {
            return _marketApis.Select(m => m.Exchange).Distinct().ToList();
        }

        public async Task<IList<CurrencyPair>> LoadCurrencyPairs(Exchange exchange)
        {
            IMarketApi marketApi = _marketApis.FirstOrDefault(m => m.Exchange == exchange);
            if (ReferenceEquals(marketApi, null))
            {
                return new List<CurrencyPair>();
            }

            return await marketApi.ReadCurrencyPairs();
        }
    }
}
