using CryptoBot.Utils.General;

namespace CryptoBot.ExchangeMonitors
{
    /// <summary>
    /// Interface definition for TradeMonitors
    /// </summary>
    public interface IExchangeMonitor
    {
        /// <summary>
        /// Get Identifier of TradeMonitor
        /// </summary>
        Exchange Exchange { get; }

        /// <summary>
        /// Subscribe on monitor for updates
        /// </summary>
        /// <param name="kind">Kind of subscription</param>
        /// <param name="currencyPair">The currency pair</param>
        /// <param name="subscriber">The subscriber</param>
        void Subscribe(SubscriptionCategory kind, CurrencyPair currencyPair, IExchangeMonitorSubscriber subscriber);

        /// <summary>
        /// Unsubscribe from monitor for updates
        /// </summary>
        /// <param name="kind">Kind of subscription</param>
        /// <param name="currencyPair">The currency pair</param>
        /// <param name="subscriber">The subscriber</param>
        void Unsubscribe(SubscriptionCategory kind, CurrencyPair currencyPair, IExchangeMonitorSubscriber subscriber);
    }
}
