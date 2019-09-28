using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ParkRate.Bl;

namespace ParkRate.Tests
{
    class RateTests
    {
        [TestCase("2019-09-26 12:30", "2019-09-26 14:00", 0)]
        [TestCase("2019-09-26 12:30", "2019-09-26 14:01", 3)]
        [TestCase("2019-09-26 12:30", "2019-09-26 15:01", 6)]
        public void ComputeRateTests(string arrivalDateTimeStr, string outDateTimeStr, decimal expectedRateValue)
        {
            DateTime arrivalDateTime = ParseDateTime(arrivalDateTimeStr);
            DateTime outDateTime = ParseDateTime(outDateTimeStr);

            Rate rate = new Rate();
            decimal rateValue = rate.Calculate(arrivalDateTime, outDateTime);

            Assert.AreEqual(expectedRateValue, rateValue);
        }

        private static DateTime ParseDateTime(string arrivalDateTimeStr)
        {
            return DateTime.ParseExact(arrivalDateTimeStr, "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
        }
    }
}
