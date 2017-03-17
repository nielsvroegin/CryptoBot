using System;
using CryptoBot.Utils.General;

namespace CryptoBot.ExchangeApi
{
    public interface IExchangeApi
    {
        /// <summary>
        /// Get the kind of Exchange this ExchangeApi is using
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
        OhlcSeries ReadOhlcSeries(CurrencyPair currencyPair, DateTime startTime, DateTime endTime, int ohlcTimeSpanSeconds);
    }
}
