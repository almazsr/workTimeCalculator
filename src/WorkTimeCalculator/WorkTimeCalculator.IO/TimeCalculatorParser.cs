using System;
using System.Text.RegularExpressions;

namespace WorkTimeCalculator.IO
{
    public class TimeCalculatorParser : ITimeCalculatorParser
    {
        private readonly IWorkDayCalendar _workDayCalendar;

        private readonly Regex _calculatorRegex = new Regex(@"(?<calculator>[WwTt]?)");

        public TimeCalculatorParser(IWorkDayCalendar workDayCalendar)
        {
            _workDayCalendar = workDayCalendar;
        }

        public object Parse(string input)
        {
            var calculatorMatch = _calculatorRegex.Match(input);
            if (!calculatorMatch.Success)
            {
                throw new InvalidOperationException();
            }

            var calculatorType = calculatorMatch.Groups["calculator"].Value.Trim().ToLower();
            switch (calculatorType)
            {
                case "w": 
                    return new WorkTimeCalculator(_workDayCalendar);
                default:
                    return new TimeCalculator();
            }
        }
    }
}