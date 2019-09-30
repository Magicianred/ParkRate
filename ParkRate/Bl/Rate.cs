using System;

namespace ParkRate.Bl
{
    public class Rate
    {
        private readonly int _payEveryMinutes;
        private readonly decimal _payAmountPerHour;
        private readonly int _payAfterMinutes;
        private readonly bool _considerAlsoTheSlackTime;
        private readonly bool _payInAdvance;

        public Rate(int payEveryMinutes = 15, decimal payAmountPerHour = 3, int payAfterMinutes = 90, bool considerAlsoTheSlackTime = true, bool payInAdvance = false)
        {
            _payInAdvance = payInAdvance;
            _payEveryMinutes = payEveryMinutes;
            _payAmountPerHour = payAmountPerHour;
            _payAfterMinutes = payAfterMinutes;
            _considerAlsoTheSlackTime = considerAlsoTheSlackTime;
        }

        public decimal Calculate(DateTime arrivalDateTime, DateTime outDateTime)
        {
            TimeSpan stayTime = outDateTime - arrivalDateTime;
            int stayTimeTotalMinutes = (int)stayTime.TotalMinutes;

            decimal rateValue = CalculateByMinutes(stayTimeTotalMinutes);
            return rateValue;
        }

        public decimal CalculateByMinutes(int stayTimeTotalMinutes)
        {
            return stayTimeTotalMinutes >= _payAfterMinutes 
                ? RateValue(stayTimeTotalMinutes, _payEveryMinutes, _payAmountPerHour)
                : 0m;
        }

        private decimal RateValue(int stayTimeTotalMinutes, int payEveryMinutes, decimal payAmountPerHour)
        {
            int timeToSubtract = _considerAlsoTheSlackTime
                ? 0
                : _payAfterMinutes;

            int payInAdvanceAmount = _payInAdvance
                ? 1
                : 0;

            // ReSharper disable once PossibleLossOfFraction
            return (((stayTimeTotalMinutes - timeToSubtract) / payEveryMinutes) + payInAdvanceAmount) * ((payAmountPerHour * payEveryMinutes )/ 60m);
        }
    }
}