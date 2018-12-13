using System;

namespace WorkTimeCalculator
{
    public class WorkDay
    {
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public WorkTime Length => new WorkTime(End - Start);
    }

	public interface IWorkDayCalendar
	{
	    WorkDay GetWorkDay(DateTime date);
	}
}