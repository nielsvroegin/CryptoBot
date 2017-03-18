using System;
using System.Threading.Tasks;
using CryptoBot.Utils.General;

namespace CryptoBot.ExchangeApi.Market
{
    public interface IMarketApi
    {
        /// <summary>
        /// Get the kind of Exchange this MarketApi is using
        /// </summary>
        Exchange Exchange { get; }

        /// <summary>
        /// Reads the ohlc series
        /// </summary>
        /// <param name="currencyPair">Currency pair to load series for</param>
        /// <param name="startTime">Start time of series</param>
        /// <param name="endTime">End time of series</param>
        /// <param name="ohlcTimeSpanSeconds">Timespan of a single ohlc item in second.</param>
        /// <returns>The loaded ohlc series</returns>
        /// <exception cref="ExchangeApiException">ExchangeApiException thrown when any error occurred during request</exception>
        Task<OhlcSeries> ReadOhlcSeries(CurrencyPair currencyPair, DateTime startTime, DateTime endTime, int ohlcTimeSpanSeconds);
    }
}
