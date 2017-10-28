using System;

namespace CryptoBot.Utils.Assertions
{
    /// <summary>
    /// Preconditions helper class
    /// </summary>
    public static class Preconditions
    {
        /// <summary>
        /// Check object not null or throw NullReferenceException
        /// </summary>
        public static T CheckNotNull<T>(T obj)
        {
            if (ReferenceEquals(obj, null))
            {
                throw new NullReferenceException();
            }

            return obj;
        }

        /// <summary>
        /// Check argument or throw argument exception
        /// </summary>
        public static void CheckArgument(bool condition)
        {
            if (!condition)
            {
                throw new ArgumentException();
            }
        }
    }
}
