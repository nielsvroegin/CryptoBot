using System;
using CryptoBot.Utils.Preconditions;

namespace CryptoBot.TickerServices.Data
{
    /// <summary>
    /// Currency pair data object
    /// </summary>
    public class CurrencyPair
    {
        /// <summary>
        /// The base currency of pair
        /// </summary>
        public Currency BaseCurrency { get; }

        /// <summary>
        /// The quote currency of pair
        /// </summary>
        public Currency QuoteCurrency { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="baseCurrency">The base currency of pair</param>
        /// <param name="quoteCurrency">The quote currency of pair</param>
        public CurrencyPair(Currency baseCurrency, Currency quoteCurrency)
        {
            BaseCurrency = Preconditions.CheckNotNull(baseCurrency);
            QuoteCurrency = Preconditions.CheckNotNull(quoteCurrency);
        }

        /// <summary>
        /// Parse a string as a currency pair. Pair is expected to be in format BASE_QUOTE (ie BTC_ETH)
        /// </summary>
        /// <param name="str">String to be parsed to currency pair</param>
        /// <returns>Converted currency pair, or null when currencies not recognized</returns>
        public static CurrencyPair Parse(String str)
        {
            String[] splittedPair = str.Split('_');

            if (splittedPair.Length != 2)
            {
                throw new ArgumentException("Expected two currencies, splitted by _");
            }

            Currency baseCurrency;
            Currency quoteCurrency;

            if (!Enum.TryParse(splittedPair[0], true, out baseCurrency))
            {
                return null;
            }

            if (!Enum.TryParse(splittedPair[1], true, out quoteCurrency))
            {
                return null;
            }

            return new CurrencyPair(baseCurrency, quoteCurrency);
        }
    }
}
