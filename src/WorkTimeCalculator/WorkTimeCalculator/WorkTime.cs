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

		//private void Fill()
		//{
		//	var totalMinutes = TotalMinutes;
		//	int additionalHours = 0;
		//	if(totalMinutes >= MinutesInHour)
		//	{
		//		additionalHours = totalMinutes / MinutesInHour;
		//	}
		//	else if(totalMinutes < 0)
		//	{
		//		additionalHours = (int)Math.Floor((double)totalMinutes / MinutesInHour);
		//		totalMinutes = MinutesInHour + totalMinutes;
		//	}
		//	Minutes = Math.Abs(totalMinutes % MinutesInHour);
		//	var totalHours = additionalHours + hours;
		//	int additionalDays = 0;
		//	if(totalHours >= HoursInDay)
		//	{
		//		additionalDays = totalHours / HoursInDay;
		//	}
		//	else if(totalHours < 0)
		//	{
		//		additionalDays = (int)Math.Floor((double)totalHours / HoursInDay);
		//		totalHours = HoursInDay + totalHours;
		//	}
		//	Hours = Math.Abs(totalHours % HoursInDay);

		//	Days = additionalDays + days;
		//}

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