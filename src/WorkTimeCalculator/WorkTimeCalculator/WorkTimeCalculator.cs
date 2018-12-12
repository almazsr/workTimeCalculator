using System;

namespace WorkTimeCalculator
{
	public class WorkTimeCalculator : IWorkTimeCalculator
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
				addedTime -= new WorkTime(WorkDayLength, GetWorkDayEndDate(date) - date);
				date = GetNextWorkDayStartDate(date);
			}
			while (addedTime.TotalMinutes() >= WorkDayLength.TotalMinutes)
			{
				var workTime = new WorkTime(WorkDayLength, _workDayCalendar.GetWorkDayLength(date));
				_workDayCalendar.GetWorkDayLength(date);
				addedTime -= workTime;
				date = GetNextWorkDayStartDate(date);
			}
			date += addedTime;
			return date;
		}

		public DateTime Add(WorkTime left, DateTime right) => Add(right, left);

		public WorkTime Subtract(DateTime left, DateTime right)
		{
			var endDate = GetEndDate(left);
			var date = endDate;
			var subtractedTime = new WorkTime(WorkDayLength, 0, 0, 0);
			if(left == date)
			{
				subtractedTime += new WorkTime(WorkDayLength, date - GetWorkDayStartDate(date));
				date = GetPreviousWorkDayEndDate(date);
			}
			while(date - WorkDayLength >= right)
			{
				var workTime = _workDayCalendar.GetWorkDayLength(date);
				subtractedTime += new WorkTime(WorkDayLength, workTime);
				date = GetPreviousWorkDayEndDate(date);
			}

			subtractedTime += new WorkTime(WorkDayLength, date - GetWorkDayStartDate(right));

			return subtractedTime;
		}

		public DateTime Subtract(DateTime left, WorkTime right)
		{
			var endDate = GetEndDate(left);
			var date = endDate;
			var subtractedTime = new WorkTime(WorkDayLength, 0, 0, 0);
			if(left == date)
			{
				subtractedTime += new WorkTime(WorkDayLength, date - GetWorkDayStartDate(date));
				date = GetPreviousWorkDayEndDate(date);
			}
			while(subtractedTime + WorkDayLength >= right)
			{
				var workTime = _workDayCalendar.GetWorkDayLength(date);
				subtractedTime += new WorkTime(WorkDayLength, workTime);
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