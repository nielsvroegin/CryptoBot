using System;
using CryptoBot.Utils.Preconditions;

namespace CryptoBot.TickerServices.Data
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
        /// Last price
        /// </summary>
        public double Last { get; private set; }

        /// <summary>
        /// Lowest ask price
        /// </summary>
        public double LowestAsk { get; private set; }

        /// <summary>
        /// Highest bid price
        /// </summary>
        public double HighestBid { get; private set; }

        /// <summary>
        /// 24 hour change
        /// </summary>
        public double PercentChange { get; private set; }

        /// <summary>
        /// 24 hour volume base
        /// </summary>
        public double BaseVolume { get; private set; }

        /// <summary>
        /// 24 hour volume quote
        /// </summary>
        public double QuoteVolume { get; private set; }

        /// <summary>
        /// Is marker frozen
        /// </summary>
        public bool IsFrozen { get; private set; }

        /// <summary>
        /// 24 hour high
        /// </summary>
        public double DayHigh { get; private set; }

        /// <summary>
        /// 24 hour low
        /// </summary>
        public double DayLow { get; private set; }

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
            return $"{nameof(CurrencyPair)}: {CurrencyPair}, {nameof(Last)}: {Last}, {nameof(LowestAsk)}: {LowestAsk}, {nameof(HighestBid)}: {HighestBid}, {nameof(PercentChange)}: {PercentChange}, {nameof(BaseVolume)}: {BaseVolume}, {nameof(QuoteVolume)}: {QuoteVolume}, {nameof(IsFrozen)}: {IsFrozen}, {nameof(DayHigh)}: {DayHigh}, {nameof(DayLow)}: {DayLow}";
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
            /// <param name="last">Last price</param>
            public Builder(CurrencyPair currencyPair, Double last)
            {
                _tickData.CurrencyPair = Preconditions.CheckNotNull(currencyPair);
                _tickData.Last = Preconditions.CheckNotNull(last);
            }

            /// <summary>
            /// Lowest ask price
            /// </summary>
            public Builder LowestAsk(double lowestAsk)
            {
                _tickData.LowestAsk = lowestAsk;
                return this;
            }

            /// <summary>
            /// Highest bid price
            /// </summary>
            public Builder HighestBid(double highestBid)
            {
                _tickData.HighestBid = highestBid;
                return this;
            }

            /// <summary>
            /// 24 hour change
            /// </summary>
            public Builder PercentChange(double percentChange)
            {
                _tickData.PercentChange = percentChange;
                return this;
            }

            /// <summary>
            /// 24 hour volume base
            /// </summary>
            public Builder BaseVolume(double baseVolume)
            {
                _tickData.BaseVolume = baseVolume;
                return this;
            }

            /// <summary>
            /// 24 hour volume quote
            /// </summary>
            public Builder QuoteVolume(double quoteVolume)
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
            public Builder DayHigh(double dayHigh)
            {
                _tickData.DayHigh = dayHigh;
                return this;
            }

            /// <summary>
            /// 24 hour low
            /// </summary>
            public Builder DayLow(double dayLow)
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
