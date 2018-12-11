using System;

namespace WorkTimeCalculator
{
	public class TimeCalculator : ITimeCalculator
	{
		public DateTime Add(DateTime left, TimeSpan right)
		{
			return left + right;
		}

		public DateTime Add(TimeSpan left, DateTime right) => Add(right, left);

		public TimeSpan Distance(DateTime left, DateTime right)
		{
			return left - right;
		}

		public DateTime Subtract(DateTime left, TimeSpan right)
		{
			return left - right;
		}	
	}
}