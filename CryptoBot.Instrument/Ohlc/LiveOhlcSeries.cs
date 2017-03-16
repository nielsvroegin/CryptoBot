using System.Collections.Generic;
using CryptoBot.Utils.General;
using CryptoBot.Utils.Preconditions;

namespace CryptoBot.Instrument.Ohlc
{
    internal sealed class LiveOhlcSeries
    {
        private const int MaxSeriesSize = 100;
        private readonly IList<OhlcItem> _ohlcItems = new List<OhlcItem>();

        public int TimeSpanSeconds { get; }

        public LiveOhlcSeries(int timeSpanSeconds)
        {
            TimeSpanSeconds = Preconditions.CheckNotNull(timeSpanSeconds);
        }

        public void AddTick(TickData tickData)
        {
            Preconditions.CheckNotNull(tickData);

            // Fill gaps, were no ticks occurred
            FillOhlcGaps(tickData);

            // Add bar or update
            AddOrUpdateOhlc(tickData);
            
            // Remove old OhlcItems
            RemoveOldOhlcItems();
        }
        
        private void FillOhlcGaps(TickData tickData)
        {
            // No gaps when first item must yet be created
            if (_ohlcItems.Count == 0)
            {
                return;
            }

            var lastOhlcItem = _ohlcItems[0];
            var timeDiff = tickData.TickTimeEpoch - lastOhlcItem.StartTimeEpoch;

            while (timeDiff > 2 * TimeSpanSeconds)
            {
                // Create new item
                var startTime = lastOhlcItem.StartTimeEpoch + TimeSpanSeconds;
                _ohlcItems.Insert(0, new OhlcItem(startTime, startTime, lastOhlcItem.Close, lastOhlcItem.Close, lastOhlcItem.Close, lastOhlcItem.Close));

                // Determine new lastOhlcItem and time diff
                lastOhlcItem = _ohlcItems[0];
                timeDiff = tickData.TickTimeEpoch - lastOhlcItem.StartTimeEpoch;
            }
        }

        private void AddOrUpdateOhlc(TickData tickData)
        {
            var startTimeEpoch = tickData.TickTimeEpoch - (tickData.TickTimeEpoch % TimeSpanSeconds);

            if (_ohlcItems.Count == 0 || _ohlcItems[0].StartTimeEpoch != startTimeEpoch)
            {
                // Add item, when no yet exists or having old start time
                AddOhlcItem(startTimeEpoch, tickData);
            }
            else
            {
                // Update Ohlc item when still in same item
                UpdateLastOhlc(tickData);
            }
        }
        
        private void AddOhlcItem(long startTimeEpoch, TickData tickData)
        {
            var newOhlcItem = new OhlcItem(startTimeEpoch, tickData.TickTimeEpoch, tickData.Last, tickData.Last, tickData.Last, tickData.Last);
            _ohlcItems.Insert(0, newOhlcItem);
        }

        private void UpdateLastOhlc(TickData tickData)
        {
            var currentOhlcItem = _ohlcItems[0];

            var open = currentOhlcItem.Open;
            var close = tickData.Last;
            var high = currentOhlcItem.High > tickData.Last ? currentOhlcItem.High : tickData.Last;
            var low = currentOhlcItem.Low < tickData.Last ? currentOhlcItem.Low : tickData.Last;

            _ohlcItems[0] = new OhlcItem(currentOhlcItem.StartTimeEpoch, tickData.TickTimeEpoch, high, low, open, close);
        }

        private void RemoveOldOhlcItems()
        {
            // Remove oldest items when execeed mas series size
            while (_ohlcItems.Count > MaxSeriesSize)
            {
                _ohlcItems.RemoveAt(_ohlcItems.Count -1);
            }
        }
    }
}
