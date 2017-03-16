

namespace CryptoBot.Utils.General
{
    /// <summary>
    /// Tick data object
    /// </summary>
    public sealed class TickData
    {
        /// <summary>
        /// Applicable currency pair for tick
        /// </summary>
        public CurrencyPair CurrencyPair { get; private set; }

        /// <summary>
        /// Time of tick
        /// </summary>
        public long TickTimeEpoch { get; private set; }

        /// <summary>
        /// Last price
        /// </summary>
        public decimal Last { get; private set; }

        /// <summary>
        /// Lowest ask price
        /// </summary>
        public decimal LowestAsk { get; private set; }

        /// <summary>
        /// Highest bid price
        /// </summary>
        public decimal HighestBid { get; private set; }

        /// <summary>
        /// 24 hour change
        /// </summary>
        public decimal PercentChange { get; private set; }

        /// <summary>
        /// 24 hour volume base
        /// </summary>
        public decimal BaseVolume { get; private set; }

        /// <summary>
        /// 24 hour volume quote
        /// </summary>
        public decimal QuoteVolume { get; private set; }

        /// <summary>
        /// Is marker frozen
        /// </summary>
        public bool IsFrozen { get; private set; }

        /// <summary>
        /// 24 hour high
        /// </summary>
        public decimal DayHigh { get; private set; }

        /// <summary>
        /// 24 hour low
        /// </summary>
        public decimal DayLow { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        private TickData()
        {
            // Create object using builder
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{nameof(CurrencyPair)}: {CurrencyPair}, {nameof(TickTimeEpoch)}: {TickTimeEpoch}, {nameof(Last)}: {Last}, {nameof(LowestAsk)}: {LowestAsk}, {nameof(HighestBid)}: {HighestBid}, {nameof(PercentChange)}: {PercentChange}, {nameof(BaseVolume)}: {BaseVolume}, {nameof(QuoteVolume)}: {QuoteVolume}, {nameof(IsFrozen)}: {IsFrozen}, {nameof(DayHigh)}: {DayHigh}, {nameof(DayLow)}: {DayLow}";
        }

        /// <summary>
        /// Builder for TickData
        /// </summary>
        public sealed class Builder
        {
            private TickData _tickData = new TickData();

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="currencyPair">Applicable currency pair for tick</param>
            /// <param name="tickTimeEpoch">Time of tick in epoch seconds</param>
            /// <param name="last">Last price</param>
            public Builder(CurrencyPair currencyPair, long tickTimeEpoch, decimal last)
            {
                _tickData.CurrencyPair = Preconditions.Preconditions.CheckNotNull(currencyPair);
                _tickData.TickTimeEpoch = Preconditions.Preconditions.CheckNotNull(tickTimeEpoch);
                _tickData.Last = Preconditions.Preconditions.CheckNotNull(last);
            }

            /// <summary>
            /// Lowest ask price
            /// </summary>
            public Builder LowestAsk(decimal lowestAsk)
            {
                _tickData.LowestAsk = lowestAsk;
                return this;
            }

            /// <summary>
            /// Highest bid price
            /// </summary>
            public Builder HighestBid(decimal highestBid)
            {
                _tickData.HighestBid = highestBid;
                return this;
            }

            /// <summary>
            /// 24 hour change
            /// </summary>
            public Builder PercentChange(decimal percentChange)
            {
                _tickData.PercentChange = percentChange;
                return this;
            }

            /// <summary>
            /// 24 hour volume base
            /// </summary>
            public Builder BaseVolume(decimal baseVolume)
            {
                _tickData.BaseVolume = baseVolume;
                return this;
            }

            /// <summary>
            /// 24 hour volume quote
            /// </summary>
            public Builder QuoteVolume(decimal quoteVolume)
            {
                _tickData.QuoteVolume = quoteVolume;
                return this;
            }

            /// <summary>
            /// Is marker frozen
            /// </summary>
            public Builder IsFrozen(bool isFrozen)
            {
                _tickData.IsFrozen = isFrozen;
                return this;
            }

            /// <summary>
            /// 24 hour high
            /// </summary>
            public Builder DayHigh(decimal dayHigh)
            {
                _tickData.DayHigh = dayHigh;
                return this;
            }

            /// <summary>
            /// 24 hour low
            /// </summary>
            public Builder DayLow(decimal dayLow)
            {
                _tickData.DayLow = dayLow;
                return this;
            }

            /// <summary>
            /// Build TickData and make sure it can't be altered by builder anymore
            /// </summary>
            /// <returns>The build TickData object</returns>
            public TickData Build()
            {
                var ret = _tickData;
                _tickData = null;
                return ret;
            }
        }
    }
}
