using System;
using System.Globalization;

namespace ParkRate.Bl
{
    public class DateTimeParser
    {
        public const string TimeFormat = "HHmm";
        public const string DateFormat = "ddMMyyyy";

        public Func<Func<DateTime>, DateTime> ParseDateTime(string dateString, string timeString)
        {
            try
            {
                DateTime leaveTime = DateTime.ParseExact($"{dateString}{timeString}", $"{DateFormat}{TimeFormat}", CultureInfo.CurrentCulture);
                return (_) => leaveTime;
            }
            catch (FormatException e)
            {
                return (orElseBranch) => orElseBranch();
            }
        }
    }
}