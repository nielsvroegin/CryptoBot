using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoBot.Bots
{
    /// <summary>
    /// Exception used when anything wrong during bot handler starting/stopping
    /// </summary>
    public class BotHandlerException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BotHandlerException() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message regarding exception</param>
        public BotHandlerException(string message) : base(message) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message regarding exception</param>
        /// <param name="inner">Exception that caused this exception</param>
        public BotHandlerException(string message, Exception inner) : base(message, inner) { }
    }
}
