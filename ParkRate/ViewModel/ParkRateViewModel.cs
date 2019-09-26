using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ParkRate.Annotations;

namespace ParkRate.ViewModel
{
    public class ParkRateViewModel : INotifyPropertyChanged
    {
        private string _arrivalTime;
        private string _arrivalDate;
        private decimal _rateValue;
        public event PropertyChangedEventHandler PropertyChanged;

        public ParkRateViewModel()
        {
            OutTime = DateTime.Now;
        }

        public String ArrivalTime
        {
            get => _arrivalTime;
            set
            {
                _arrivalTime = value;
                OnPropertyChanged(nameof(ArrivalTime));
                RateValue = 0;
            }
        }

        public String ArrivalDate
        {
            get => _arrivalDate;
            set
            {
                _arrivalDate = value;
                OnPropertyChanged(nameof(ArrivalDate));
                RateValue = 0;
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

        public DateTime OutTime { get; set; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
