using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private string _arrivalTimeStr;
        private string _arrivalDateStr;
        private decimal _rateValue;
        private string _leaveDateStr;
        private string _leaveTimeStr;
        private Brush _arrivalTimeColor;
        public event PropertyChangedEventHandler PropertyChanged;

        public static readonly Brush HelpColor = Brushes.DimGray;
        private Brush _leaveTimeColor;
        private readonly DateTimeParser _dateTimeParser;
        private DateTime _arrivalDateTime;
        private DateTime _leaveDateTime;
        private string _leaveDateTimeStr;
        private string _arrivalDateTimeStr;

        public ParkRateViewModel()
        {
            var now = DateTime.Now;
            PropertyChanged += (sender, args) => { UpdateFields(args); };

            _arrivalTimeColor = HelpColor;
            _leaveTimeColor = HelpColor;
            _dateTimeParser = new DateTimeParser();
            
            ArrivalTimeStr = now.ToString(DateTimeParser.TimeFormat);
            ArrivalDateStr = now.ToString(DateTimeParser.DateFormat);
            LeaveTimeStr = _arrivalTimeStr;
            LeaveDateStr = _arrivalDateStr;
        }

        private void UpdateFields(PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(ArrivalDateTime))
            {
                ArrivalDateTimeStr = ArrivalDateTime.ToString("g");
            }

            if (args.PropertyName == nameof(LeaveDateTime))
            {
                LeaveDateTimeStr = LeaveDateTime.ToString("g");
            }
        }

        public String ArrivalDateTimeStr
        {
            get => _arrivalDateTimeStr;
            private set
            {
                _arrivalDateTimeStr = value;
                OnPropertyChanged(nameof(ArrivalDateTimeStr));
            }
        }

        public String LeaveDateTimeStr
        {
            get => _leaveDateTimeStr;
            private set
            {
                _leaveDateTimeStr = value;
                OnPropertyChanged(nameof(LeaveDateTimeStr));
            }
        }
        public DateTime ArrivalDateTime
        {
            get => _arrivalDateTime;
            private set
            {
                _arrivalDateTime = value;
                OnPropertyChanged(nameof(ArrivalDateTime));
            }
        }

        public DateTime LeaveDateTime
        {
            get => _leaveDateTime;
            private set
            {
                _leaveDateTime = value;
                OnPropertyChanged(nameof(LeaveDateTime));
            }
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
            ArrivalDateTime = _dateTimeParser.ParseDateTime(ArrivalDateStr, ArrivalTimeStr)(() =>
            {
                ArrivalTimeColor = Brushes.Red;
                return DateTime.Now;
            });

            LeaveTimeColor = HelpColor;
            LeaveDateTime = _dateTimeParser.ParseDateTime(LeaveDateStr, LeaveTimeStr)(() =>
            {
                LeaveTimeColor = Brushes.Red;
                return DateTime.Now;
            });

            Rate rate = new Rate();
            return rate.Calculate(ArrivalDateTime, LeaveDateTime);
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

        public string LeaveTimeStr
        {
            get => _leaveTimeStr;
            set
            {
                _leaveTimeStr = value;
                OnPropertyChanged(nameof(LeaveTimeStr));
                UpdateRateValue();
            }
        }

        public string LeaveDateStr
        {
            get => _leaveDateStr;
            set
            {
                _leaveDateStr = value;
                OnPropertyChanged(nameof(LeaveDateStr));
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

        public Brush LeaveTimeColor
        {
            get => _leaveTimeColor;
            set
            {
                _leaveTimeColor = value;
                OnPropertyChanged(nameof(LeaveTimeColor));
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
