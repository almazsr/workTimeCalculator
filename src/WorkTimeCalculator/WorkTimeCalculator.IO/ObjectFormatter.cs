using System;
using System.Globalization;

namespace WorkTimeCalculator.IO
{
    public class ObjectFormatter : IObjectFormatter
    {
        public string FormatObject(object obj)
        {
            if (obj is DateTime date)
            {
                return date.ToString("dd.MM.yy HH:mm", CultureInfo.InvariantCulture);
            }

            if (obj is TimeSpan time)
            {
                var format = time.Days >= 1 ? @"d\.hh\:mm" : @"hh\:mm";
                return time.ToString(format);
            }

	        if (obj is WorkTime workTime)
	        {
		        var minutes = workTime.Minutes();
		        return $"{workTime.Days()}d {workTime.Hours()}{(double)minutes / WorkTime.MinutesInHour:.#}h";
	        }

            throw new ArgumentException("Invalid obj type", nameof(obj));
        }
    }
}