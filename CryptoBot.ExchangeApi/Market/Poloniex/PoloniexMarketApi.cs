using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using CryptoBot.Utils.Assertions;
using CryptoBot.Utils.General;
using CryptoBot.Utils.Helpers;
using Newtonsoft.Json.Linq;


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

        public async Task<IList<CurrencyPair>> ReadCurrencyPairs()
        {
            // Load response
            const string command = "return24hVolume";
            var response = await _dataRetriever.PerformRequest(ServerUrl, command, null);
            
            // Convert JSON to JToken
            JToken data = JToken.Parse(response);

            // Extract CurrencyPairs from JToken
            var currencyPairs = new List<CurrencyPair>();
            foreach (var token in data)
            {
                var property = token as JProperty;

                // Continue when not a property or name doesn't contain the currency seperator
                if (ReferenceEquals(property, null) || !property.Name.Contains("_"))
                {
                    continue;
                }

                // Convert name to currency pair
                var currencyPair = CurrencyPair.Parse(property.Name);
                if (!ReferenceEquals(currencyPair, null))
                {
                    currencyPairs.Add(currencyPair);
                }
            }

            return currencyPairs;
        }
    }
}
