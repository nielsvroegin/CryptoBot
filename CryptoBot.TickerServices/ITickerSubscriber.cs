using CryptoBot.TickerServices.Data;

namespace CryptoBot.TickerServices
{
    /// <summary>
    /// Interface definition for TickerService subscriber
    /// </summary>
    public interface ITickerSubscriber
    {
        /// <summary>
        /// Receiver method for new TickData
        /// </summary>
        /// <param name="tickData">The TickData</param>
        void OnTick(TickData tickData);
    }
}
