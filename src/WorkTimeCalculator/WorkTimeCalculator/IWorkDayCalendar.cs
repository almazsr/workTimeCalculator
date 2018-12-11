using System;

namespace WorkTimeCalculator
{
	public interface IWorkDayCalendar
	{
		TimeSpan GetWorkDayLength(DateTime date);
	}
}