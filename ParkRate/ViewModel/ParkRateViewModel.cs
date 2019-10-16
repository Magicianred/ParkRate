using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Media;
using ParkRate.Annotations;
using ParkRate.Bl;

namespace ParkRate.ViewModel
{
    public class ParkRateViewModel : INotifyPropertyChanged
    {
        private readonly ParkRateConfig _config;
        private const string RateOptionProperty = "RateOption";
        private const string RateOptionPropertyUpdatedByConfig = "RateOptionPropertyUpdatedByConfig";
        private string _arrivalTimeStr;
        private string _arrivalDateStr;
        private string _rateValue;
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
        private List<RateExample> _exampleList;
        private bool _paySlackTime;
        private bool _payInAdvance;
        private int _payEveryMinutes;
        private decimal _payAmountPerHour;
        private int _slackTime;
        public string ConfigFilePath;
        private string _payAmountPerHourStr;

        public ParkRateViewModel() : this(new ParkRateConfig())
        {}

        public ParkRateViewModel(ParkRateConfig config)
        {
            _config = config;
            var now = DateTime.Now;
            PropertyChanged += (sender, args) => { UpdateFields(args); };

            _arrivalTimeColor = HelpColor;
            _leaveTimeColor = HelpColor;
            _dateTimeParser = new DateTimeParser();

            _paySlackTime = config.PaySlackTime;
            _payInAdvance = config.PayInAdvance;
            _payEveryMinutes = config.PayEveryMinutes;
            _payAmountPerHour = StringToDecimal(config.PayAmountPerHour, _payAmountPerHour);
            _payAmountPerHourStr = config.PayAmountPerHour;
            _slackTime = config.SlackTime;

            LeaveTimeStr = now.ToString(DateTimeParser.TimeFormat);
            LeaveDateStr = now.ToString(DateTimeParser.DateFormat);
            ArrivalTimeStr = ComputeArrivalTime(now);
            ArrivalDateStr = _leaveDateStr;

            ComputeExamples();
        }

        private string ComputeArrivalTime(DateTime now)
        {
            return (now - TimeSpan.FromMinutes(SlackTime)).ToString(DateTimeParser.TimeFormat);
        }

        public void UpdateWithConfig(ParkRateConfig config)
        {
            PaySlackTime = config.PaySlackTime;
            PayInAdvance = config.PayInAdvance;
            PayEveryMinutes = config.PayEveryMinutes;
            PayAmountPerHourStr = config.PayAmountPerHour;
            SlackTime = config.SlackTime;
            ArrivalTimeStr = string.Empty;
        }

        private void ComputeExamples()
        {
            ExampleList = new List<RateExample>
            {
                new RateExample
                {
                    Title = "Fino a 15 minuti",
                    Value = RateToEuro(14),
                },
                new RateExample
                {
                    Title = "da 15 minuti",
                    Value = RateToEuro(15),
                },
                new RateExample
                {
                    Title = "fino a 30 minuti",
                    Value = RateToEuro(29),
                },
                new RateExample
                {
                    Title = "fino a 1 ora",
                    Value = RateToEuro(59),
                },
                new RateExample
                {
                    Title = "fino a 90 minuti",
                    Value = RateToEuro(89),
                },
                new RateExample
                {
                    Title = "da 90 minuti",
                    Value = RateToEuro(90),
                },
                new RateExample
                {
                    Title = "fino a 2 ore",
                    Value = RateToEuro(119),
                },
                new RateExample
                {
                    Title = "fino a 3 ore",
                    Value = RateToEuro(179),
                }
            };
        }

        private string RateToEuro(int stayTimeTotalMinutes)
        {
            Rate rate = GetRate();
            return $"{rate.CalculateByMinutes(stayTimeTotalMinutes):F2} Euro";
        }

        private Rate GetRate()
        {
            return new Rate(
                PayEveryMinutes,
                PayAmountPerHour,
                SlackTime,
                PaySlackTime,
                PayInAdvance);
        }

        private void UpdateFields(PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(ArrivalDateTime))
            {
                ArrivalDateTimeStr = ArrivalDateTime.ToString("g");
                return;
            }

            if (args.PropertyName == nameof(LeaveDateTime))
            {
                LeaveDateTimeStr = LeaveDateTime.ToString("g");
                return;
            }

            if (args.PropertyName == RateOptionProperty)
            {
                SaveConfig();
                UpdateUi();
                return;
            }

            if (args.PropertyName == nameof(PayAmountPerHourStr))
            {
                PayAmountPerHour = StringToDecimal(PayAmountPerHourStr, PayAmountPerHour);
                return;
            }
        }

        private decimal StringToDecimal(string value, decimal defaultValue)
        {
            if (Decimal.TryParse(value, out var decimalValue))
            {
                return decimalValue;
            }

            return defaultValue;
        }

        private void UpdateUi()
        {
            ComputeExamples();
            UpdateRateValue();
        }

        private void SaveConfig()
        {
            if (ConfigFilePath != null)
            {
                _config.SlackTime = SlackTime;
                _config.PaySlackTime = PaySlackTime;
                _config.PayAmountPerHour = PayAmountPerHourStr;
                _config.PayEveryMinutes = PayEveryMinutes;
                _config.PayInAdvance = PayInAdvance;
               _config.ToXml(ConfigFilePath);
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
            RateValue = $"€ {ComputeRateValue():F2}";
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

            Rate rate = GetRate();
            return rate.Calculate(ArrivalDateTime, LeaveDateTime);
        }

        public String RateValue
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

        public List<RateExample> ExampleList
        {
            get => _exampleList;
            set
            {
                _exampleList = value;
                OnPropertyChanged(nameof(ExampleList));
            }
        }

        public bool PaySlackTime
        {
            get => _paySlackTime;
            set
            {
                _paySlackTime = value;
                OnPropertyChanged(nameof(PaySlackTime));
                OnPropertyChanged(RateOptionProperty);
            }
        }

        public bool PayInAdvance
        {
            get => _payInAdvance;
            set
            {
                _payInAdvance = value;
                OnPropertyChanged(nameof(PayInAdvance));
                OnPropertyChanged(RateOptionProperty);
            }
        }

        public int PayEveryMinutes
        {
            get => _payEveryMinutes;
            set
            {
                _payEveryMinutes = Math.Max(value, 1);
                OnPropertyChanged(nameof(PayEveryMinutes));
                OnPropertyChanged(RateOptionProperty);
            }
        }

        public decimal PayAmountPerHour
        {
            get => _payAmountPerHour;
            set
            {
                var valueToAssign = Math.Max(value, 0);
                if (valueToAssign != _payAmountPerHour)
                {
                    _payAmountPerHour = valueToAssign;
                    OnPropertyChanged(nameof(PayAmountPerHour));
                }
            }
        }

        public int SlackTime
        {
            get => _slackTime;
            set
            {
                _slackTime = Math.Max(value, 0);
                OnPropertyChanged(nameof(SlackTime));
                OnPropertyChanged(RateOptionProperty);
            }
        }

        public string PayAmountPerHourStr
        {
            get => _payAmountPerHourStr;
            set
            {
                _payAmountPerHourStr = value;
                OnPropertyChanged(nameof(PayAmountPerHourStr));
                OnPropertyChanged(RateOptionProperty);
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RateExample
    {
        public String Title { get; set; }
        public String Value { get; set; }
    }
}
