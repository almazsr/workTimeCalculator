using System;

namespace WorkTimeCalculator
{
	public interface ITimeCalculator
	{
		DateTime Add(DateTime left, TimeSpan right);
		TimeSpan Distance(DateTime left, DateTime right);
		DateTime Subtract(DateTime left, TimeSpan right);
	}
}