using System;

namespace WorkTimeCalculator
{
	public struct WorkTime
	{
		public const int MinutesInHour = 60;

		public WorkTime(int workDayLength, int days, int hours, int minutes)
		{
			WorkDayLength = workDayLength;

			var totalMinutes = minutes;
			int additionalHours = 0;
			if (totalMinutes >= MinutesInHour)
			{
				additionalHours = totalMinutes / MinutesInHour;
			}
			else if (totalMinutes < 0)
			{
				additionalHours = (int)Math.Floor((double)totalMinutes / MinutesInHour);
				totalMinutes = MinutesInHour + totalMinutes;
			}
			Minutes = Math.Abs(totalMinutes % MinutesInHour);
			var totalHours = additionalHours + hours;
			int additionalDays = 0;
			if (totalHours >= WorkDayLength)
			{
				additionalDays = totalHours / workDayLength;
			}
			else if (totalHours < 0)
			{
				additionalDays = (int)Math.Floor((double)totalHours / WorkDayLength);
				totalHours = workDayLength + totalHours;
			}
			Hours = Math.Abs(totalHours % workDayLength);

			Days = additionalDays + days;
		}

		public WorkTime(int workDayLength, TimeSpan timeSpan) : this(workDayLength, timeSpan.Days, timeSpan.Hours,
			timeSpan.Minutes)
		{

		}

		public WorkTime(TimeSpan workDayLength, TimeSpan timeSpan) : this(workDayLength.Hours, timeSpan)
		{

		}

		public WorkTime(TimeSpan workDayLength, int days, int hours, int minutes) : this(workDayLength.Hours, days, hours, minutes)
		{

		}

		public static implicit operator TimeSpan(WorkTime time)
		{
			return new TimeSpan(time.Days, time.Hours, time.Minutes, 0);
		}

		public static WorkTime operator +(WorkTime left, int right)
		{
			var workDayLength = left.WorkDayLength;

			return new WorkTime(workDayLength, left.Days, left.Hours + right,
				left.Minutes);
		}

		public static bool operator >=(WorkTime left, WorkTime right)
		{
			if(left.WorkDayLength != right.WorkDayLength)
			{
				throw new InvalidOperationException();
			}

			return left.TotalMinutes() >= right.TotalMinutes();
		}

		public int TotalMinutes()
		{
			return Days * WorkDayLength * MinutesInHour + Hours * MinutesInHour + Minutes;
		}

		public static bool operator <=(WorkTime left, WorkTime right)
		{
			if(left.WorkDayLength != right.WorkDayLength)
			{
				throw new InvalidOperationException();
			}

			return left.TotalMinutes() <= right.TotalMinutes();
		}

		public static WorkTime operator +(WorkTime left, WorkTime right)
		{
			if(left.WorkDayLength != right.WorkDayLength)
			{
				throw new InvalidOperationException();
			}

			var workDayLength = left.WorkDayLength;

			return new WorkTime(workDayLength, left.Days + right.Days, left.Hours + right.Hours,
				left.Minutes + right.Minutes);
		}

		public static WorkTime operator -(WorkTime left, WorkTime right)
		{
			if(left.WorkDayLength != right.WorkDayLength)
			{
				throw new InvalidOperationException();
			}

			var workDayLength = left.WorkDayLength;

			return new WorkTime(workDayLength, left.Days - right.Days, left.Hours - right.Hours,
				left.Minutes - right.Minutes);
		}

		public int Days { get; }
		public int Hours { get; }
		public int Minutes { get; }
		public int WorkDayLength { get; }
	}
}