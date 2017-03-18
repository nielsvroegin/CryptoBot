using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoBot.ExchangeApi
{
    /// <summary>
    /// Exception used when anything wrong during ExchangeApi request
    /// </summary>
    public class ExchangeApiException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ExchangeApiException() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message regarding exception</param>
        public ExchangeApiException(string message) : base(message) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message regarding exception</param>
        /// <param name="inner">Exception that caused this exception</param>
        public ExchangeApiException(string message, Exception inner) : base(message, inner) { }
    }
}
