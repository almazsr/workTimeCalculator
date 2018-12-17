using System;
using System.Threading.Tasks;

namespace WorkTimeCalculator
{
	public interface IWorkDayCalendar
	{
		TimePeriod GetWorkDay(DateTime date);
	}
}