using Microsoft.Extensions.Logging;

namespace CryptoBot.Utils.Logging
{
    /// <summary>
    /// Helper class for application logging
    /// </summary>
    public static class ApplicationLogging
    {
        /// <summary>
        /// The factory for creating logggers
        /// </summary>
        public static ILoggerFactory LoggerFactory { get; } = new LoggerFactory();

        /// <summary>
        /// Helper method to create logger
        /// </summary>
        /// <typeparam name="T">Logger target class</typeparam>
        /// <returns>The created logger</returns>
        public static ILogger CreateLogger<T>() =>
          LoggerFactory.CreateLogger<T>();
    }
}
