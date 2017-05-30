using System;
using CryptoBot.Utils.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CryptoBot.Utils.UnitTests.Helpers
{
    [TestClass]
    public class DateTimeHelperTests
    {
        [TestMethod]
        public void TestToUnixTime()
        {
            Assert.AreEqual(0, DateTimeHelper.ToUnixTime(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)));
            Assert.AreEqual(1490522551,
                DateTimeHelper.ToUnixTime(new DateTime(2017, 3, 26, 10, 2, 31, DateTimeKind.Utc)));
        }

        [TestMethod]
        public void TestToDateTime()
        {
            Assert.AreEqual(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc), DateTimeHelper.ToDateTime(0));
            Assert.AreEqual(new DateTime(2017, 3, 26, 10, 2, 31, DateTimeKind.Utc),
                DateTimeHelper.ToDateTime(1490522551));
        }
    }
}
