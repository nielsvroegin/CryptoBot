using System;

namespace CryptoBot.Utils.ServiceHandler
{
    /// <summary>
    /// Interface definition for startable/stopable services
    /// </summary>
    public interface IManagedService
    {
        /// <summary>
        /// Start service
        /// </summary>
        void Start();

        /// <summary>
        /// Stop service
        /// </summary>
        void Stop();
    }
}
