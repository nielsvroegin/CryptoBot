﻿using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Read the supported currency pairs of exchange
        /// </summary>
        /// <returns>List of supported currency pairs</returns>
        Task<IList<CurrencyPair>> ReadCurrencyPairs();

        /// <summary>
        /// Read historic trades on exchange
        /// </summary>
        /// <param name="currencyPair">Currency pair to load historic trades for</param>
        /// <param name="startTime">Start time of historic trades</param>
        /// <param name="endTime">End time of historic traders</param>
        /// <returns>List of historic trades, ordered from old to new</returns>
        Task<IList<Trade>> ReadHistoricTrades(CurrencyPair currencyPair, DateTime startTime, DateTime endTime);
    }
}
