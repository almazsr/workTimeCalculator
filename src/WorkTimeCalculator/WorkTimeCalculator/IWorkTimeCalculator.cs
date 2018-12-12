using System;

namespace WorkTimeCalculator
{
	public interface IWorkTimeCalculator
	{
		DateTime Add(DateTime left, WorkTime right);
		DateTime Add(WorkTime right, DateTime left);
		WorkTime Subtract(DateTime left, DateTime right);
		DateTime Subtract(DateTime left, WorkTime right);
	}
}