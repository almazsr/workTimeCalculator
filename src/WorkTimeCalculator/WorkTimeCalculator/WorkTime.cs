using System;

namespace WorkTimeCalculator
{
	public struct WorkTime
	{
		public WorkTime(int minutes)
		{
			TotalMinutes = minutes;			
		}

		public WorkTime(int hours, int minutes) : this(hours * MinutesInHour + minutes)
		{

		}

		public WorkTime(int days, int hours, int minutes) : this(
			days * HoursInDay * MinutesInHour + hours * MinutesInHour + minutes)
		{

		}

		public static int HoursInDay = 8;
		public const int MinutesInHour = 60;

		public int TotalMinutes;

		public int Minutes()
		{
			return TotalMinutes % MinutesInHour;
		}

		public int Hours()
		{
			return TotalMinutes / MinutesInHour % HoursInDay;
		}

		public int Days()
		{
			return TotalMinutes / MinutesInHour / HoursInDay;
		}
	}
}