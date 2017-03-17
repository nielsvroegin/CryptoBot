using CryptoBot.Utils.Assertions;

namespace CryptoBot.Utils.General
{
    /// <summary>
    /// Ohlc item data object
    /// </summary>
    public sealed class OhlcItem
    {
        /// <summary>
        /// Gets the start time of the OhlcItem
        /// </summary>
        public long StartTimeEpoch { get; }

        /// <summary>
        /// Gets the last update time of OhlcItem
        /// </summary>
        public long LastUpdateEpoch { get; }

        /// <summary>
        /// Highest price
        /// </summary>
        public decimal High { get; }

		/// <summary>
		/// Lowest price
		/// </summary>
		public decimal Low { get; }

		/// <summary>
		/// Opening price
		/// </summary>
		public decimal Open { get; }

		/// <summary>
		/// Closing price
		/// </summary>
		public decimal Close { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public OhlcItem(long startTimeEpoch, long lastUpdateEpoch, decimal high, decimal low, decimal open, decimal close)
        {
            StartTimeEpoch = Preconditions.CheckNotNull(startTimeEpoch);
            LastUpdateEpoch = Preconditions.CheckNotNull(lastUpdateEpoch);
            High = Preconditions.CheckNotNull(high);
            Low = Preconditions.CheckNotNull(low);
            Open = Preconditions.CheckNotNull(open);
            Close = Preconditions.CheckNotNull(close);
        }
    }
}
