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
        [TestCase("2019-09-26 12:30", "2019-09-26 13:59", 0)]
        [TestCase("2019-09-26 12:30", "2019-09-26 14:00", 4.5)]
        [TestCase("2019-09-26 12:30", "2019-09-26 14:01", 4.5)]
        [TestCase("2019-09-26 12:30", "2019-09-26 15:00", 7.5)]
        [TestCase("2019-09-26 12:30", "2019-09-26 14:14", 4.50)]
        [TestCase("2019-09-26 12:30", "2019-09-26 14:15", 5.25)]
        public void ComputeRateTests(string arrivalDateTimeStr, string outDateTimeStr, decimal expectedRateValue)
        {
            DateTime arrivalDateTime = ParseDateTime(arrivalDateTimeStr);
            DateTime outDateTime = ParseDateTime(outDateTimeStr);

            Rate rate = new Rate();
            decimal rateValue = rate.Calculate(arrivalDateTime, outDateTime);

            Assert.AreEqual(expectedRateValue, rateValue);
        }

        [TestCase(89, 0)]
        [TestCase(90, 4.5)]
        [TestCase(91, 4.5)]
        [TestCase(150, 7.5)]
        [TestCase(104, 4.50)]
        [TestCase(105, 5.25)]
        public void ComputeRateTests_ByMinutes(int minutes, decimal expectedRateValue)
        {
            Rate rate = new Rate();
            decimal rateValue = rate.CalculateByMinutes(minutes);

            Assert.AreEqual(expectedRateValue, rateValue);
        }

        [TestCase(89, 0)]
        [TestCase(90, 3)]
        [TestCase(91, 3)]
        [TestCase(150, 6)]
        [TestCase(104, 3)]
        [TestCase(105, 3)]
        [TestCase(180, 9)]
        public void ComputeRateTests_ByMinutes_EveryHour(int minutes, decimal expectedRateValue)
        {
            Rate rate = new Rate(60);
            decimal rateValue = rate.CalculateByMinutes(minutes);

            Assert.AreEqual(expectedRateValue, rateValue);
        }

        [TestCase(89, 0)]
        [TestCase(90, 0)]
        [TestCase(91, 0)]
        [TestCase(150, 3)]
        [TestCase(104, 0)]
        [TestCase(105, 0.75)]
        [TestCase(180, 4.5)]
        public void ComputeRateTests_ByMinutes_DoNotConsider_FirstSlackTime(int minutes, decimal expectedRateValue)
        {
            Rate rate = new Rate(considerAlsoTheSlackTime: false);
            decimal rateValue = rate.CalculateByMinutes(minutes);

            Assert.AreEqual(expectedRateValue, rateValue);
        }

        [TestCase(0, 0)]
        [TestCase(1, 0)]
        [TestCase(60, 3)]
        [TestCase(14, 0)]
        [TestCase(15, 0.75)]
        [TestCase(90, 4.5)]
        public void ComputeRateTests_ByMinutes_DoNotConsider_WithoutSlackTime(int minutes, decimal expectedRateValue)
        {
            Rate rate = new Rate(payAfterMinutes: 0, considerAlsoTheSlackTime: false);
            decimal rateValue = rate.CalculateByMinutes(minutes);

            Assert.AreEqual(expectedRateValue, rateValue);
        }

        [TestCase(0, 0.75)]
        [TestCase(1, 0.75)]
        [TestCase(60, 3.75)]
        [TestCase(14, 0.75)]
        [TestCase(15, 1.5)]
        [TestCase(90, 5.25)]
        public void ComputeRateTests_ByMinutes_DoNotConsider_WithoutSlackTime_PayInAdvance(int minutes, decimal expectedRateValue)
        {
            Rate rate = new Rate(payAfterMinutes: 0, considerAlsoTheSlackTime: false, payInAdvance: true);
            decimal rateValue = rate.CalculateByMinutes(minutes);

            Assert.AreEqual(expectedRateValue, rateValue);
        }

        private static DateTime ParseDateTime(string arrivalDateTimeStr)
        {
            return DateTime.ParseExact(arrivalDateTimeStr, "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
        }
    }
}
