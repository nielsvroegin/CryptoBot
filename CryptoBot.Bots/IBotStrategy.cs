using System;
using System.Collections.Generic;
using System.Text;
using CryptoBot.Utils.General;

namespace CryptoBot.Bots
{
    /// <summary>
    /// Interface definition of a bot strategy
    /// </summary>
    public interface IBotStrategy
    {
        /// <summary>
        /// Exchange to use for strategy
        /// </summary>
        Exchange Exchange { get; }

        /// <summary>
        /// Currency pair to use for strategy
        /// </summary>
        CurrencyPair CurrencyPair { get; }

        /// <summary>
        /// Initialize bot strategy
        /// </summary>
        void Init();

        /// <summary>
        /// Deinitialize bot strategy
        /// </summary>
        void Deinit();

        /// <summary>
        /// Handle a new Tick
        /// </summary>
        void HandleTick(TickData tickData);
    }
}
