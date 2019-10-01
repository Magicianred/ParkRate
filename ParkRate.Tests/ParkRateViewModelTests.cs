﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
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
            Assert.That(viewModel.OutTimeStr, Is.EqualTo(DateTime.Now.ToString("HHmm")));
            Assert.That(viewModel.OutDateStr, Is.EqualTo(DateTime.Now.ToString("ddMMyyyy")));
        }

        [TestCase("26092019", "1230", "26092019", "1359", 0, Description = "up to 90 minutes later, no charge")]
        [TestCase("26092019", "1230", "26092019", "1400", 4.5, Description = "91 minutes later, start paying")]
        public void Given_HourOfArrival_IExpect_ARateAmount(
            string arrivalDate,
            string arrivalTime,
            string outDate,
            string outTime, 
            decimal expectedRate)
        {
            ParkRateViewModel viewModel = new ParkRateViewModel
            {
                ArrivalDateStr = arrivalDate,
                ArrivalTimeStr = arrivalTime,
                OutDateStr = outDate,
                OutTimeStr = outTime
            };

            Assert.AreEqual(expectedRate, viewModel.RateValue);
        }

        [Test]
        public void GivenAString_AsTime_CouldNotBe_InTheRightFormat()
        {
            ParkRateViewModel viewModel = new ParkRateViewModel()
            {
                ArrivalTimeStr = "banana"
            };
            Assert.AreEqual(Brushes.Red, viewModel.ArrivalTimeColor);
            Assert.AreEqual(0, viewModel.RateValue);
        }

        [Test]
        public void GivenAString_AsTime_IfItIs_InTheRightFormat_Then_NoErrorIsSignaled()
        {
            ParkRateViewModel viewModel = new ParkRateViewModel()
            {
                ArrivalTimeStr = "0615"
            };
            Assert.AreEqual(ParkRateViewModel.HelpColor, viewModel.ArrivalTimeColor);
        }
    }
}
