namespace CryptoBot.TickerServices
{
    /// <summary>
    /// Interface definition for TickerServices
    /// </summary>
    public interface ITickerService
    {
        /// <summary>
        /// Subscribe on service for ticker updates
        /// </summary>
        /// <param name="subscriber">The subscriber</param>
        void Subscribe(ITickerSubscriber subscriber);

        /// <summary>
        /// Unsubscribe from service for ticker updates
        /// </summary>
        /// <param name="subscriber">The subscriber</param>
        void Unsubscribe(ITickerSubscriber subscriber);
    }
}
