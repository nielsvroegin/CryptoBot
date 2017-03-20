using System;

namespace CryptoBot.Instrument.Static
{
    /// <summary>
    /// Exception used when anything wrong during instrument calculation of OhlcSeries 
    /// </summary>
    public class OhlcCalculationException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OhlcCalculationException() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message regarding exception</param>
        public OhlcCalculationException(string message) : base(message) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message regarding exception</param>
        /// <param name="inner">Exception that caused this exception</param>
        public OhlcCalculationException(string message, Exception inner) : base(message, inner) { }
    }
}
