using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using CryptoBot.Utils.Assertions;
using CryptoBot.Utils.General;
using CryptoBot.Utils.Helpers;


namespace CryptoBot.ExchangeApi.Market.Poloniex
{
    public sealed class PoloniexMarketApi : IMarketApi
    {
        private const string ServerUrl = "https://poloniex.com/public";
        private readonly IDataRetriever _dataRetriever;

		public Exchange Exchange => Exchange.Poloniex;

        public PoloniexMarketApi() : this(new DataRetriever()) { }

        public PoloniexMarketApi(IDataRetriever dataRetriever)
        {
            _dataRetriever = Preconditions.CheckNotNull(dataRetriever);
        }
        
        public async Task<OhlcSeries> ReadOhlcSeries(CurrencyPair currencyPair, DateTime startTime, DateTime endTime, int ohlcTimeSpanSeconds)
        {
            // Request chart data
            const string command = "returnChartData";
            var parameters = new Dictionary<string, string>
            {
                // TODO: Map global currency pair to poloniex currency pair instead of expecting these to be equal
                {"currencyPair", currencyPair.ToString()},
                {"start", DateTimeHelper.ToUnixTime(startTime).ToString()},
                {"end", DateTimeHelper.ToUnixTime(endTime).ToString()},
                {"period", ohlcTimeSpanSeconds.ToString()}
            };

            var chartData = await _dataRetriever.PerformRequest<IList<MarketChartData>>(ServerUrl, command, parameters);

            // Convert chart data to OhlcSeries
            var ohlcItems = ImmutableList.CreateBuilder<OhlcItem>();
            foreach (var chartItem in chartData.Reverse())
            {
                var startOhlc = DateTimeHelper.ToUnixTime(chartItem.Time);
                var ohlcItem = new OhlcItem(startOhlc, startOhlc, chartItem.High, chartItem.Low, chartItem.Open, chartItem.Close);

                ohlcItems.Add(ohlcItem);
            }

            return new OhlcSeries(ohlcTimeSpanSeconds, ohlcItems.ToImmutable());
        }
    }
}
