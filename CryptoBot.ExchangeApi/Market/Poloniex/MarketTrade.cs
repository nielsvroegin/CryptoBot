using System;
using System.Globalization;
using Newtonsoft.Json;
using static CryptoBot.Utils.General.Trade;

namespace CryptoBot.ExchangeApi.Market.Poloniex
{
    internal sealed class MarketTrade
    {
        [JsonProperty("date")]
        // ReSharper disable once UnusedMember.Local
        private string TimeInternal
        {
            set => Date = DateTime.ParseExact(value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        }
        public DateTime Date { get; set; }

        [JsonProperty("type")]
        public OrderType Type { get; set; }

        [JsonProperty("rate")]
        public decimal Rate { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("total")]
        public decimal Total { get; set; }
    }
}
