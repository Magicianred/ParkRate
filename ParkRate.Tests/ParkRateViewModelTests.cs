using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ParkRate.ViewModel;

namespace ParkRate.Tests
{
    public class ParkRateViewModelTests
    {
        [Test]
        public void OutTime_Defaults_ToNow()
        {
            ParkRateViewModel viewModel = new ParkRateViewModel();
            Assert.That(viewModel.OutTime, Is.EqualTo(DateTime.Now).Within(1).Seconds);
        }

        [Test]
        public void Given_HourOfArrival_IExpect_ARateAmount()
        {
            int day = 26;
            int month = 9;
            int year = 2019;
            int hour = 12;
            int minutes = 15;
            ParkRateViewModel viewModel = new ParkRateViewModel
            {
                ArrivalDate = $"{day}{month}{year}",
                ArrivalTime = $"{hour}{minutes}",
                OutTime = new DateTime(year, month, day, hour + 1, minutes, 0)
            };

            decimal expectedRate = 0;
            Assert.AreEqual(expectedRate, viewModel.RateValue);
        }

    }
}
