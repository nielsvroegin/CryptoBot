using log4net;

namespace CryptoBot.Utils.Logging
{
    /// <summary>
    /// Helper class for application logging
    /// </summary>
    public static class ApplicationLogging
    {
        /// <summary>
        /// Helper method to create logger
        /// </summary>
        /// <typeparam name="T">Logger target class</typeparam>
        /// <returns>The created logger</returns>
        public static ILog CreateLogger<T>() => LogManager.GetLogger(typeof(T));
    }
}
