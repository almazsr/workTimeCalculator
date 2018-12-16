using System;

namespace WorkTimeCalculator
{
	public struct TimePeriod
	{
		public DateTime Start;
		public DateTime End;
		public TimeSpan Length => End - Start;
	}
}