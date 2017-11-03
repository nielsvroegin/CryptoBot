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
        /// <param name="currencyPair">The currency pair</param>
        /// <param name="subscriber">The subscriber</param>
        void Subscribe(CurrencyPair currencyPair, IExchangeMonitorSubscriber subscriber);

        /// <summary>
        /// Unsubscribe from monitor for updates
        /// </summary>
        /// <param name="currencyPair">The currency pair</param>
        /// <param name="subscriber">The subscriber</param>
        void Unsubscribe(CurrencyPair currencyPair, IExchangeMonitorSubscriber subscriber);
    }
}
