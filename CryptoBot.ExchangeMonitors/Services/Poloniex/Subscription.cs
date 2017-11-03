using CryptoBot.Utils.Assertions;
using CryptoBot.Utils.General;

namespace CryptoBot.ExchangeMonitors.Services.Poloniex
{
    internal class Subscription
    {
        /// <summary>
        /// Applicable curreny pair
        /// </summary>
        public CurrencyPair CurrencyPair { get; }

        /// <summary>
        /// The subscriber
        /// </summary>
        public IExchangeMonitorSubscriber Subscriber { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Subscription(CurrencyPair currencyPair, IExchangeMonitorSubscriber subscriber)
        {
            CurrencyPair = Preconditions.CheckNotNull(currencyPair);
            Subscriber = Preconditions.CheckNotNull(subscriber);
        }
    }
}
