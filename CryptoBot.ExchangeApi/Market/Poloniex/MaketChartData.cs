using System;
using CryptoBot.Utils.Helpers;
using Newtonsoft.Json;

namespace CryptoBot.ExchangeApi.Market.Poloniex
{
    internal sealed class MarketChartData
    {
        [JsonProperty("date")]
        // ReSharper disable once UnusedMember.Local
        private long TimeInternal
        {
            set { Time = DateTimeHelper.ToDateTime(value); }
        }
        public DateTime Time { get; set; }

        [JsonProperty("open")]
        public decimal Open { get; set; }
        [JsonProperty("close")]
        public decimal Close { get; set; }

        [JsonProperty("high")]
        public decimal High { get; set; }
        [JsonProperty("low")]
        public decimal Low { get; set; }

        [JsonProperty("volume")]
        public decimal VolumeBase { get; set; }
        [JsonProperty("quoteVolume")]
        public decimal VolumeQuote { get; set; }

        [JsonProperty("weightedAverage")]
        public decimal WeightedAverage { get; set; }
    }
}
