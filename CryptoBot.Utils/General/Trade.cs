using System;
using CryptoBot.Utils.Assertions;

namespace CryptoBot.Utils.General
{
    public class Trade
    {
        public enum OrderType
        {
            Buy,
            Sell
        }

        public DateTime Date { get; }

        public OrderType Type { get; }

        public decimal Rate { get; }

        public decimal Amount { get; }

        public decimal Total { get; }

        public Trade(DateTime date, OrderType type, decimal rate, decimal amount, decimal total)
        {
            Date = Preconditions.CheckNotNull(date);
            Type = Preconditions.CheckNotNull(type);
            Rate = Preconditions.CheckNotNull(rate);
            Amount = Preconditions.CheckNotNull(amount);
            Total = Preconditions.CheckNotNull(total);
        }
    }
}
