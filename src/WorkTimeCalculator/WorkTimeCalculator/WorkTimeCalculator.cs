using System;

namespace WorkTimeCalculator
{
	public struct WorkTime
	{
		public static implicit operator WorkTime(TimeSpan time)
		{
			return new WorkTime
			{
				Days = time.Days,
				Hours = time.Hours,
				Minutes = time.Minutes
			};
		}

		public static WorkTime operator +(WorkTime left, WorkTime right)
		{
			var minutes = left.Minutes + right.Minutes;
			var additionalHours = 0;
			if (minutes >= 60)
			{
				additionalHours = minutes / 60;
				minutes = minutes % 60;
			}

			var hours = additionalHours + left.Hours + right.Hours;
			var additionalDays = 0;
			if(hours >= left.WorkDayLength)
			{
				additionalDays = minutes / left.WorkDayLength;
				hours = hours % left.WorkDayLength;
			}

			var days = additionalDays + left.Days + right.Days;

		}

		public static WorkTime operator +(TimeSpan time, WorkTime workTime)
		{
			return new WorkTime { Days = }
		}

		public int Days { get; set; }
		public int Hours { get; set; }
		public int Minutes { get; set; }
		public int WorkDayLength { get; set; }
	}

	public class WorkTimeCalculator : ITimeCalculator
	{
		private readonly IWorkDayCalendar _workDayCalendar;

		public WorkTimeCalculator(IWorkDayCalendar workDayCalendar, TimeSpan workDayStart, TimeSpan workDayEnd)
		{
			_workDayCalendar = workDayCalendar;
			WorkDayStart = workDayStart;
			WorkDayEnd = workDayEnd;
		}

		public TimeSpan WorkDayStart { get; }
		public TimeSpan WorkDayEnd { get; }
		public TimeSpan WorkDayLength => WorkDayEnd - WorkDayStart;

		private DateTime GetStartDate(DateTime date)
		{
			if (date.TimeOfDay < WorkDayStart)
			{
				return GetWorkDayStartDate(date);
			}
			if (date.TimeOfDay >= WorkDayEnd)
			{
				return GetNextWorkDayStartDate(date);
			}
			return date;
		}

		private DateTime GetEndDate(DateTime date)
		{
			if(date.TimeOfDay < WorkDayStart)
			{
				return GetPreviousWorkDayEndDate(date);
			}
			if(date.TimeOfDay >= WorkDayEnd)
			{
				return GetWorkDayEndDate(date);
			}
			return date;
		}

		public DateTime Add(DateTime left, WorkTime right)
		{
			var startDate = GetStartDate(left);
			var date = startDate;
			var addedTime = right;
			if (left == date)
			{
				addedTime -= GetWorkDayEndDate(date) - date;
				date = GetNextWorkDayStartDate(date);
			}
			while (addedTime >= WorkDayLength)
			{
				var workTime = _workDayCalendar.GetWorkDayLength(date);
				addedTime -= workTime;
				date = GetNextWorkDayStartDate(date);
			}
			date += addedTime;
			return date;
		}

		public DateTime Add(TimeSpan left, DateTime right) => Add(right, left);

		public TimeSpan Subtract(DateTime left, DateTime right)
		{
			var endDate = GetEndDate(left);
			var date = endDate;
			var subtractedTime = TimeSpan.Zero;
			if(left == date)
			{
				subtractedTime += date - GetWorkDayStartDate(date);
				date = GetPreviousWorkDayEndDate(date);
			}
			while(date - WorkDayLength >= right)
			{
				var workTime = _workDayCalendar.GetWorkDayLength(date);
				subtractedTime += workTime;
				date = GetPreviousWorkDayEndDate(date);
			}

			subtractedTime += date - GetWorkDayStartDate(right);

			return subtractedTime;
		}

		public DateTime Subtract(DateTime left, TimeSpan right)
		{
			var endDate = GetEndDate(left);
			var date = endDate;
			var subtractedTime = TimeSpan.Zero;
			if(left == date)
			{
				subtractedTime += date - GetWorkDayStartDate(date);
				date = GetPreviousWorkDayEndDate(date);
			}
			while(subtractedTime + WorkDayLength >= right)
			{
				var workTime = _workDayCalendar.GetWorkDayLength(date);
				subtractedTime += workTime;
				date = GetPreviousWorkDayEndDate(date);
			}

			date -= right - subtractedTime;

			return date;
	    }

	    private DateTime GetWorkDayEndDate(DateTime date) => date.Date.Add(WorkDayEnd);
	    private DateTime GetWorkDayStartDate(DateTime date) => date.Date.Add(WorkDayStart);
	    private DateTime GetPreviousWorkDayEndDate(DateTime date) => GetWorkDayEndDate(date.AddDays(-1));
	    private DateTime GetNextWorkDayStartDate(DateTime date) => GetWorkDayStartDate(date.AddDays(1));
    }
}