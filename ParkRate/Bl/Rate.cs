using System;

namespace ParkRate.Bl
{
    public class Rate
    {
        public decimal Calculate(DateTime arrivalDateTime, DateTime outDateTime)
        {
            TimeSpan stayTime = outDateTime - arrivalDateTime;

            int stayTimeTotalMinutes = (int)stayTime.TotalMinutes;
            var rateValue = stayTimeTotalMinutes > 90 ? (stayTimeTotalMinutes / 60) * 3 : 0;
            return rateValue;
        }
    }
}