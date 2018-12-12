using System;

namespace WorkTimeCalculator
{
	public interface ITimeCalculator
	{
		DateTime Add(DateTime left, TimeSpan right);
		DateTime Add(TimeSpan right, DateTime left);
		TimeSpan Subtract(DateTime left, DateTime right);
		DateTime Subtract(DateTime left, TimeSpan right);
	}
}