using System;

namespace WorkTimeCalculator
{
	public struct TimePeriod
	{
		public DateTime Start;
		public DateTime End;

		public DateTime Min(DateTime left, DateTime right)
		{
			return left.Ticks > right.Ticks ? right : left;
		}

		public DateTime Max(DateTime left, DateTime right)
		{
			return left.Ticks < right.Ticks ? right : left;
		}

		public TimePeriod Intersection(TimePeriod left, TimePeriod right)
		{
			return new TimePeriod {Start = Max(left.Start, right.Start), End = Min(left.End, right.End)};
		}
	}

	public class WorkTimeCalculator : IWorkTimeCalculator
	{
		private readonly IWorkDayCalendar _workDayCalendar;

		public WorkTimeCalculator(IWorkDayCalendar workDayCalendar)
		{
			_workDayCalendar = workDayCalendar;
		}

		private DateTime GetWorkDate(DateTime date)
		{
			if(date.TimeOfDay < _workDayCalendar.GetWorkDay(date).Start)
			{
				return GetWorkDayStartDate(date);
			}
			if(date.TimeOfDay >= _workDayCalendar.GetWorkDay(date).End)
			{
				return GetWorkDayEndDate(date);
			}
			return date;
		}

		public DateTime Add(DateTime left, WorkTime right)
		{
			var startDate = GetStartDate(left);
			var date = startDate;
			var addedTime = right;
			if (left == date)
			{
				addedTime -= new WorkTime(WorkDayLength, GetWorkDayEndDate(date) - date);
				date = GetNextWorkDayStartDate(date);
			}
			while (addedTime.TotalMinutes() >= WorkDayLength.TotalMinutes)
			{
				var workTime = new WorkTime(WorkDayLength, _workDayCalendar.GetWorkDay(date));
				_workDayCalendar.GetWorkDay(date);
				addedTime -= workTime;
				date = GetNextWorkDayStartDate(date);
			}
			date += addedTime;
			return date;
		}

		public DateTime Add(WorkTime left, DateTime right) => Add(right, left);

	    private WorkTime IntersectWithWorkDay(DateTime date)
	    {
	        var workDay = _workDayCalendar.GetWorkDay(date);

            if (date.TimeOfDay < WorkDayStart)
	        {
	            return WorkTime.Zero(WorkDayLength);
	        }

            if (date.TimeOfDay > WorkDayEnd)
	        {
                return new WorkTime(WorkDayLength, WorkDayLength);
	        }

	        return workTime.Length - new WorkTime(WorkDayLength, date - GetWorkDayStartDate(date));
	    }

        public WorkTime Subtract(DateTime left, DateTime right)
		{
		    if (Math.Abs((right-left).TotalHours) < 24 && 
		        left.TimeOfDay >= WorkDayEnd && right.TimeOfDay <= WorkDayStart)
		    {
                return WorkTime.Zero(WorkDayLength);
            }

		    if (left == date)
			{
				subtractedTime += new WorkTime(WorkDayLength, date - GetWorkDayStartDate(date));
				date = GetPreviousWorkDayEndDate(date);
		    }
            
		    var additionalTime = IntersectWithWorkDay(left);

		    var date = GetNextWorkDayStartDate(left);
            var subtractedTime = WorkTime.Zero(WorkDayLength);
		    var previousEndDate = GetPreviousWorkDayEndDate(date);

            while (date - WorkDayLength >= right)
            {
                date = previousEndDate;
                var workTime = _workDayCalendar.GetWorkDay(date);                
				subtractedTime += new WorkTime(WorkDayLength, workTime);
				date = GetPreviousWorkDayEndDate(date);
                previousEndDate = GetPreviousWorkDayEndDate(date);
            }

            subtractedTime += new WorkTime(WorkDayLength, Subtract(date, right));

            return subtractedTime;
		}

		public DateTime Subtract(DateTime left, WorkTime right)
		{
			var endDate = GetWorkDate(left);
			var date = endDate;
			var subtractedTime = WorkTime.Zero(WorkDayLength);
            if (left == date)
			{
				subtractedTime += new WorkTime(WorkDayLength, date - GetWorkDayStartDate(date));
				date = GetPreviousWorkDayEndDate(date);
			}
			while(subtractedTime + WorkDayLength <= right)
			{
				var workTime = _workDayCalendar.GetWorkDay(date);
				subtractedTime += new WorkTime(WorkDayLength, workTime);
				date = GetPreviousWorkDayEndDate(date);
		    }

            date -= right - subtractedTime;

		    if (date.Hour == WorkDayEnd.Hours)
		    {
		        date = GetNextWorkDayStartDate(date);
		    }

            return date;
	    }

	    private DateTime GetWorkDayEndDate(DateTime date) => date.Date.Add(WorkDayEnd);
	    private DateTime GetWorkDayStartDate(DateTime date) => date.Date.Add(WorkDayStart);
	    private DateTime GetPreviousWorkDayEndDate(DateTime date) => GetWorkDayEndDate(date.AddDays(-1));
	    private DateTime GetNextWorkDayStartDate(DateTime date) => GetWorkDayStartDate(date.AddDays(1));
    }
}