using System;

namespace WorkTimeCalculator
{
	public class WorkTimeCalculator : IWorkTimeCalculator
	{
		private readonly IWorkDayCalendar _workDayCalendar;

		public WorkTimeCalculator(IWorkDayCalendar workDayCalendar)
		{
			_workDayCalendar = workDayCalendar;
		}

		public static DateTime Min(DateTime left, DateTime right)
		{
			return left.Ticks > right.Ticks ? right : left;
		}

		public static DateTime Max(DateTime left, DateTime right)
		{
			return left.Ticks < right.Ticks ? right : left;
		}

		public DateTime Add(DateTime left, WorkTime right)
		{
			if(right.TotalMinutes < 0)
			{
				throw new ArgumentException("Right should be positive", nameof(right));
			}

			var date = left;
			var nextDate = date;
			var minutes = right.TotalMinutes;
			while(minutes > 0)
			{
				date = nextDate;
				var workDay = _workDayCalendar.GetWorkDay(date);
				var startDate = Min(Max(workDay.Start, date), workDay.End);
				date = Min(workDay.End, startDate.AddMinutes(Math.Min(workDay.Length.TotalMinutes, minutes)));
				minutes -= (int) (date - startDate).TotalMinutes;
				if (date == workDay.End)
				{
					nextDate = date.Date.AddDays(1);
				}
			}
			return date;
		}

		public DateTime Add(WorkTime left, DateTime right) => Add(right, left);

		public WorkTime Subtract(DateTime left, DateTime right)
		{
			if (right > left)
			{
				throw new ArgumentException("Right should be less than left", nameof(right));
			}
			
			var date = left;
			var minutes = 0;
			while(date > right)
			{
				var workDay = _workDayCalendar.GetWorkDay(date);
				var endDate = Max(Min(workDay.End, date), workDay.Start);
				date = Max(workDay.Start, right);
				minutes += (int)(endDate - date).TotalMinutes;
				if(date == workDay.Start)
				{
					date = date.Date.AddMinutes(-1);
				}
			}

			return new WorkTime(minutes);
		}

		public DateTime Subtract(DateTime left, WorkTime right)
		{
			if(right.TotalMinutes < 0)
			{
				throw new ArgumentException("Right should be positive", nameof(right));
			}

			var date = left;
			var nextDate = date;
			var minutes = right.TotalMinutes;
			while(minutes > 0)
			{
				date = nextDate;
				var workDay = _workDayCalendar.GetWorkDay(date);
				var endDate = Max(Min(workDay.End, date), workDay.Start);
				date = Max(workDay.Start, endDate.AddMinutes(-Math.Min(workDay.Length.TotalMinutes, minutes)));
				minutes -= (int)(endDate - date).TotalMinutes;
				if(date == workDay.Start)
				{
					nextDate = date.Date.AddMinutes(-1);
				}
			}

			return date;
		}
    }
}