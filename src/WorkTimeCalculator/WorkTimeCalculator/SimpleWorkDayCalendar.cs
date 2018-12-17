using System;
using System.Threading.Tasks;

namespace WorkTimeCalculator
{
	public class SimpleWorkDayCalendar : IWorkDayCalendar
	{
		public SimpleWorkDayCalendar(TimeSpan workDayStart, TimeSpan workDayEnd)
		{
			WorkDayStart = workDayStart;
			WorkDayEnd = workDayEnd;
		}

		public TimeSpan WorkDayStart { get; }
		public TimeSpan WorkDayEnd { get; }

		public TimePeriod GetWorkDay(DateTime date)
		{
			if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
			{
				return new TimePeriod {Start = date, End = date};
			}

			return new TimePeriod {Start = date.Date.Add(WorkDayStart), End = date.Date.Add(WorkDayEnd) };
		}
	}
}