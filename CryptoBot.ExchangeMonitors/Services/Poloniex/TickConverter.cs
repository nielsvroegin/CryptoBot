using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoBot.Utils.General;
using WampSharp.V2;
using CryptoBot.Utils.Helpers;

namespace CryptoBot.ExchangeMonitors.Services.Poloniex
{
    internal static class TickConverter
    {
        public static TickData Convert(Exchange exchange, ISerializedValue[] serializedData)
        {
            // Determine applicable currency pair
            var currencyPair = GetCurrencyPair(serializedData);

            // Convert data to TickData
            var tickData = new TickData.Builder(exchange, currencyPair, DateTimeHelper.ToUnixTime(DateTime.Now), serializedData[1].Deserialize<decimal>())
                .LowestAsk(serializedData[2].Deserialize<decimal>())
                .HighestBid(serializedData[3].Deserialize<decimal>())
                .PercentChange(serializedData[4].Deserialize<decimal>())
                .BaseVolume(serializedData[5].Deserialize<decimal>())
                .QuoteVolume(serializedData[6].Deserialize<decimal>())
                .IsFrozen(serializedData[7].Deserialize<byte>() > 0)
                .DayHigh(serializedData[8].Deserialize<decimal>())
                .DayLow(serializedData[9].Deserialize<decimal>())
                .Build();

            return tickData;
        }

        public static CurrencyPair GetCurrencyPair(ISerializedValue[] serializedData)
        {
            var currencyPairStr = serializedData[0].Deserialize<string>();
            var currencyPair = CurrencyPair.Parse(currencyPairStr);
            
            return currencyPair;
        }
    }
}
