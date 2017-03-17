using System;
using System.Net;
using CryptoBot.Utils.Assertions;
using CryptoBot.Utils.General;

namespace CryptoBot.ExchangeApi.Poloniex
{
    public sealed class PoloniexApi : IExchangeApi
    {
        private readonly IWebRequestCreate _webRequestFactory;

		public Exchange Exchange => Exchange.Poloniex;

        public PoloniexApi(IWebRequestCreate webRequestFactory)
        {
            _webRequestFactory = Preconditions.CheckNotNull(webRequestFactory);
        }

        public OhlcSeries ReadOhlcSeries(CurrencyPair currencyPair, DateTime startTime, DateTime endTime, int ohlcTimeSpanSeconds)
        {
            throw new NotImplementedException();
        }
    }
}
