using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoBot.ExchangeMonitors
{
    /// <summary>
    /// Kind of subscription
    /// </summary>
    public enum SubscriptionCategory
    {
        /// <summary>
        /// Ticks subscription
        /// </summary>
        Ticks,

        /// <summary>
        /// Trades subscription
        /// </summary>
        Trades,

        /// <summary>
        /// Orders subscription
        /// </summary>
        Orders
    }
}
