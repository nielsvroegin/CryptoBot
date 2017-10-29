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
        /// <param name="subscriber">The subscriber</param>
        void Subscribe(IExchangeMonitorSubscriber subscriber);

        /// <summary>
        /// Unsubscribe from monitor for updates
        /// </summary>
        /// <param name="subscriber">The subscriber</param>
        void Unsubscribe(IExchangeMonitorSubscriber subscriber);
    }
}
