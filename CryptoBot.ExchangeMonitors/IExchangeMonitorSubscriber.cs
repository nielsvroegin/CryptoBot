using CryptoBot.Utils.General;

namespace CryptoBot.ExchangeMonitors
{
    /// <summary>
    /// Interface definition for ExchangeMonitor subscriber
    /// </summary>
    public interface IExchangeMonitorSubscriber
    {
        /// <summary>
        /// Receiver method for new ticks
        /// </summary>
        /// <param name="tick">The trade</param>
        /// <param name="exchange">Exchange that deliver this tick</param>
        void OnTick(Exchange exchange, TickData tick);
    }
}
