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
        /// Kind of subscription
        /// </summary>
        public SubscriptionCategory Kind { get; }

        /// <summary>
        /// The subscriber
        /// </summary>
        public IExchangeMonitorSubscriber Subscriber { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Subscription(CurrencyPair currencyPair, SubscriptionCategory kind, IExchangeMonitorSubscriber subscriber)
        {
            CurrencyPair = Preconditions.CheckNotNull(currencyPair);
            Kind = Preconditions.CheckNotNull(kind);
            Subscriber = Preconditions.CheckNotNull(subscriber);
        }
    }
}
