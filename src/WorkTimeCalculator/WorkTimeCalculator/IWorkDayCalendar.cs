using System;

namespace WorkTimeCalculator
{
	public interface IWorkDayCalendar
	{
		TimePeriod GetWorkDay(DateTime date);
	}
}