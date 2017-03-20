using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoBot.Utils.Helpers
{
    /// <summary>
    /// Helper for DateTime conversions
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// Convert DateTime to unix epoch
        /// </summary>
        /// <param name="date">Date to convert</param>
        /// <returns>Epoch time in seconds</returns>
        public static long ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date.ToUniversalTime() - epoch).TotalSeconds);
        }

        /// <summary>
        /// Convert epoch seconds to DateTime object
        /// </summary>
        /// <param name="epochSeconds">Epoch time in seconds</param>
        /// <returns>DateTime object</returns>
        public static DateTime ToDateTime(long epochSeconds)
        {
            var unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            var unixTimeStampInTicks = epochSeconds * TimeSpan.TicksPerSecond;
            return new DateTime(unixStart.Ticks + unixTimeStampInTicks, System.DateTimeKind.Utc);
        }
    }
}
