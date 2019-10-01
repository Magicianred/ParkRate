﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ParkRate.Annotations;
using ParkRate.Bl;

namespace ParkRate.ViewModel
{
    public class ParkRateViewModel : INotifyPropertyChanged
    {
        private const string TimeFormat = "HHmm";
        private const string DateFormat = "ddMMyyyy";

        private string _arrivalTimeStr;
        private string _arrivalDateStr;
        private decimal _rateValue;
        private string _outDateStr;
        private string _outTimeStr;
        private Brush _arrivalTimeColor;
        public event PropertyChangedEventHandler PropertyChanged;

        public static readonly Brush HelpColor = Brushes.DimGray;

        public ParkRateViewModel()
        {
            var now = DateTime.Now;
            
            _arrivalTimeStr = now.ToString(TimeFormat);
            _arrivalDateStr = now.ToString(DateFormat);
            _outTimeStr = _arrivalTimeStr;
            _outDateStr = _arrivalDateStr;

            _arrivalTimeColor = HelpColor;
        }

        public String ArrivalTimeStr
        {
            get => _arrivalTimeStr;
            set
            {
                _arrivalTimeStr = value;
                OnPropertyChanged(nameof(ArrivalTimeStr));
                UpdateRateValue();
            }
        }

        public String ArrivalDateStr
        {
            get => _arrivalDateStr;
            set
            {
                _arrivalDateStr = value;
                OnPropertyChanged(nameof(ArrivalDateStr));
                UpdateRateValue();
            }
        }

        private void UpdateRateValue()
        {
            RateValue = ComputeRateValue();
        }

        private decimal ComputeRateValue()
        {
            ArrivalTimeColor = HelpColor;
            DateTime arrivalTime = ParseDateTime(ArrivalDateStr, ArrivalTimeStr)(() =>
            {
                ArrivalTimeColor = Brushes.Red;
                return DateTime.Now;
            });

            DateTime outTime = ParseDateTime(OutDateStr, OutTimeStr)(() => DateTime.Now);

            Rate rate = new Rate();
            return rate.Calculate(arrivalTime, outTime);
        }

        private Func<Func<DateTime>, DateTime> ParseDateTime(string dateString, string timeString)
        {
            try
            {
                DateTime ouTime = DateTime.ParseExact($"{dateString}{timeString}", $"{DateFormat}{TimeFormat}", CultureInfo.CurrentCulture);
                return (_) => ouTime;
            }
            catch (FormatException e)
            {
                return (orElseBranch) => orElseBranch();
            }
        }

        public Decimal RateValue
        {
            get => _rateValue;
            private set
            {
                _rateValue = value;
                OnPropertyChanged(nameof(RateValue));
            }
        }

        public string OutTimeStr
        {
            get => _outTimeStr;
            set
            {
                _outTimeStr = value;
                OnPropertyChanged(nameof(OutTimeStr));
                UpdateRateValue();
            }
        }

        public string OutDateStr
        {
            get => _outDateStr;
            set
            {
                _outDateStr = value;
                OnPropertyChanged(nameof(OutDateStr));
                UpdateRateValue();
            }
        }

        public Brush ArrivalTimeColor
        {
            get => _arrivalTimeColor;
            set
            {
                _arrivalTimeColor = value;
                OnPropertyChanged(nameof(ArrivalTimeColor));
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
