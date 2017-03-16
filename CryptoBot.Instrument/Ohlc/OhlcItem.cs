using System;
using System.Collections.Generic;
using System.Text;
using CryptoBot.Utils.Preconditions;

namespace CryptoBot.Instrument.Ohlc
{
    public sealed class OhlcItem
    {
        public long StartTimeEpoch { get; }
        public long LastUpdateEpoch { get; }
        public decimal High { get; }
        public decimal Low { get; }
        public decimal Open { get; }
        public decimal Close { get; }

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
