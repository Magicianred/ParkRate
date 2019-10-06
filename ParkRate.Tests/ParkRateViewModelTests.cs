using System;
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
            Assert.That(viewModel.LeaveTimeStr, Is.EqualTo(DateTime.Now.ToString("HHmm")));
            Assert.That(viewModel.LeaveDateStr, Is.EqualTo(DateTime.Now.ToString("ddMMyyyy")));
        }

        [TestCase("26092019", "1230", "26092019", "1359", "0.00", Description = "up to 90 minutes later, no charge")]
        [TestCase("26092019", "1230", "26092019", "1400", "5.25", Description = "91 minutes later, start paying")]
        public void Given_HourOfArrival_IExpect_ARateAmount(
            string arrivalDate,
            string arrivalTime,
            string outDate,
            string outTime, 
            string expectedRate)
        {
            ParkRateViewModel viewModel = new ParkRateViewModel
            {
                ArrivalDateStr = arrivalDate,
                ArrivalTimeStr = arrivalTime,
                LeaveDateStr = outDate,
                LeaveTimeStr = outTime
            };

            Assert.AreEqual(expectedRate, viewModel.RateValue);
        }

        [Test]
        public void OnInit_Arrival_IsSetToNow_AndLeave_IsSetTo_NowMinusSlackTime()
        {
            ParkRateViewModel viewModel = new ParkRateViewModel();
            DateTime expectedDefaultLeave = DateTime.Now;
            expectedDefaultLeave = new DateTime(expectedDefaultLeave.Year, expectedDefaultLeave.Month, expectedDefaultLeave.Day, expectedDefaultLeave.Hour, expectedDefaultLeave.Minute, 0);
            DateTime expectedDefaultArrival = expectedDefaultLeave - TimeSpan.FromMinutes(viewModel.SlackTime);

            Assert.That(viewModel.ArrivalDateTime, Is.EqualTo(expectedDefaultArrival).Within(TimeSpan.FromSeconds(2)), "Arrival Time");
            Assert.That(viewModel.LeaveDateTime, Is.EqualTo(expectedDefaultLeave).Within(TimeSpan.FromSeconds(2)), "Leave Time");
        }

        [Test]
        public void GivenAString_AsArrivalTime_CouldNotBe_InTheRightFormat()
        {
            ParkRateViewModel viewModel = new ParkRateViewModel()
            {
                ArrivalTimeStr = "banana",
                ArrivalDateStr = "12122019"
            };
            Assert.AreEqual(Brushes.Red, viewModel.ArrivalTimeColor);
            Assert.AreEqual("0.00", viewModel.RateValue);
        }

        [Test]
        public void GivenAString_ArrivalTime_IGet_TheExpected_StringRepresentation()
        {
            ParkRateViewModel viewModel = new ParkRateViewModel
            {
                ArrivalTimeStr = "0615",
                ArrivalDateStr = "12122019"
            };
            Assert.AreEqual("12/12/2019 06:15", viewModel.ArrivalDateTimeStr);
        }

        [Test]
        public void GivenAString_LeaveTime_IGet_TheExpected_StringRepresentation()
        {
            ParkRateViewModel viewModel = new ParkRateViewModel()
            {
                LeaveTimeStr = "0615",
                LeaveDateStr = "12122019"
            };
            Assert.AreEqual("12/12/2019 06:15", viewModel.LeaveDateTimeStr);
        }

        [Test]
        public void GivenAString_AsArrivalTime_IfItIs_InTheRightFormat_Then_NoErrorIsSignaled()
        {
            ParkRateViewModel viewModel = new ParkRateViewModel()
            {
                ArrivalTimeStr = "0615",
                ArrivalDateStr = "12122019"
            };
            Assert.AreEqual(ParkRateViewModel.HelpColor, viewModel.ArrivalTimeColor);
        }

        [Test]
        public void GivenAString_AsLeaveTime_CouldNotBe_InTheRightFormat()
        {
            ParkRateViewModel viewModel = new ParkRateViewModel()
            {
                LeaveTimeStr = "banana",
                LeaveDateStr = "12122019"
            };
            Assert.AreEqual(Brushes.Red, viewModel.LeaveTimeColor);
        }

        [Test]
        public void GivenAString_AsLeaveTime_IfItIs_InTheRightFormat_Then_NoErrorIsSignaled()
        {
            ParkRateViewModel viewModel = new ParkRateViewModel()
            {
                LeaveTimeStr = "0615",
                LeaveDateStr = "12122019"
            };
            Assert.AreEqual(ParkRateViewModel.HelpColor, viewModel.LeaveTimeColor);
        }
    }
}
