using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using CryptoBot.Utils.Assertions;

namespace CryptoBot.Utils.General
{
    /// <summary>
    /// Immutable Ohlc Series data object
    /// </summary>
    public sealed class OhlcSeries
    {
        /// <summary>
        /// Gets the time span per OhlcItem
        /// </summary>
		public int TimeSpanSeconds { get; }

        /// <summary>
        /// Gets the ohlc items
        /// </summary>
        public ImmutableList<OhlcItem> OhlcItems { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="timeSpanSeconds">Time span in seconds per OhlcItem</param>
        /// <param name="ohlcItems">List of ohlc items</param>
        public OhlcSeries(int timeSpanSeconds, IList<OhlcItem> ohlcItems)
        {
            Preconditions.CheckNotNull(ohlcItems);

            TimeSpanSeconds = Preconditions.CheckNotNull(timeSpanSeconds);
            OhlcItems = ImmutableList.CreateRange(ohlcItems);
        }
    }
}
