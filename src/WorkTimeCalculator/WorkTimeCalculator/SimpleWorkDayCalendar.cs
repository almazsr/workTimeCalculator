using System;

namespace WorkTimeCalculator
{
	public class SimpleWorkDayCalendar : IWorkDayCalendar
	{
		public TimePeriod GetWorkDay(DateTime date)
		{
			if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
			{
				return new TimePeriod {Start = date, End = date};
			}

			return new TimePeriod {Start = date.Date.AddHours(10), End = date.Date.AddHours(18) };
		}
	}
}