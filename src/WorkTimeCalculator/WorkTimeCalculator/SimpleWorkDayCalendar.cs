using System;

namespace WorkTimeCalculator
{
	public class SimpleWorkDayCalendar : IWorkDayCalendar
	{
		public TimeSpan StandardWorkDay => TimeSpan.FromHours(8);

		public TimeSpan GetWorkDayLength(DateTime date)
		{
			return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday
				? TimeSpan.Zero
				: StandardWorkDay;
		}
	}
}