using System;
using System.Collections.Generic;

namespace WorkTimeCalculator
{
	public interface IWorkTimeCalculator
	{
		DateTime Add(DateTime left, WorkTime right);
		DateTime Add(WorkTime right, DateTime left);
		IEnumerable<TimePeriod> GetSchedule(DateTime start, WorkTime time);
		WorkTime Subtract(DateTime left, DateTime right);
		DateTime Subtract(DateTime left, WorkTime right);
	}
}