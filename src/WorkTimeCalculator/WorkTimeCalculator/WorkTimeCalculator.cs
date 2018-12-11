using System;

namespace WorkTimeCalculator
{
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

		private DateTime GetWorkDayEndDate(DateTime date) => date.Date.Add(WorkDayEnd);
		private DateTime GetWorkDayStartDate(DateTime date) => date.Date.Add(WorkDayStart);
		private DateTime GetPreviousWorkDayEndDate(DateTime date) => GetWorkDayEndDate(date.AddDays(-1));
		private DateTime GetNextWorkDayStartDate(DateTime date) => GetWorkDayStartDate(date.AddDays(1));

		public DateTime Add(DateTime left, TimeSpan right)
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

		public TimeSpan Distance(DateTime left, DateTime right)
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
	}
}