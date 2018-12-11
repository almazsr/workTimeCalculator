using System;
using System.Text.RegularExpressions;

namespace WorkTimeCalculator.IO
{
    public class TimeCalculatorParser : ITimeCalculatorParser
    {
        private readonly IWorkDayCalendar _workDayCalendar;

        private readonly Regex _calculatorRegex = new Regex(@"(?<calculator>[WwTt]?)");
        private readonly Regex _workTimeCalculatorRegex = new Regex(@"w\((?<workDayStart>.+),{1}(?<workDayEnd>.+)\)");

        public TimeCalculatorParser(IWorkDayCalendar workDayCalendar)
        {
            _workDayCalendar = workDayCalendar;
        }

        public ITimeCalculator Parse(string input)
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
                    var workTimeCalculatorMatch = _workTimeCalculatorRegex.Match(input);
                    if (!workTimeCalculatorMatch.Success)
                    {
                        throw new InvalidOperationException();
                    }

                    var workDayStart = TimeSpan.Parse(workTimeCalculatorMatch.Groups["workDayStart"].Value.Trim());
                    var workDayEnd = TimeSpan.Parse(workTimeCalculatorMatch.Groups["workDayEnd"].Value.Trim());
                    return new WorkTimeCalculator(_workDayCalendar, workDayStart, workDayEnd);
                default:
                    return new TimeCalculator();
            }
        }
    }
}