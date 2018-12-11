using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace WorkTimeCalculator.IO
{
    public class TimeExpressionParser : ITimeExpressionParser
    {
        private readonly int _workDayLength;
        private readonly Regex _expressionRegex;
        private readonly Regex _timeRegex;
        
        public TimeExpressionParser(int workDayLength)
        {
            _workDayLength = workDayLength;
            _expressionRegex = new Regex(@"(?<calculator>[Ww]\(.+\)|[Tt]|) (?<left>.+)(?<sign>[+-]{1})(?<right>.+)", RegexOptions.Compiled);
            _timeRegex = new Regex(@"(?<hours>\d+):(?<minutes>[0-5][0-9])", RegexOptions.Compiled);
        }
        
        private bool TryParseDate(string input, out DateTime date)
        {
            if (DateTime.TryParseExact(input, new[]{"dd.MM.yy HH:mm", "dd.MM.yy"},
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date1))
            {
                date = date1;
                return true;
            }

            date = default(DateTime);
            return false;
        }

        private bool TryParseTime(string input, out TimeSpan time)
        {
            if (TryParseTimeNormal(input, out TimeSpan time1))
            {
                if (time1.Days >= 1)
                {
                    time = TimeSpan.FromHours(time1.Days * _workDayLength + time1.Hours);
                }
                return true;
            }
            if (TryParseTimeMoreThan24Hours(input, out TimeSpan time2))
            {
                time = time2;
                return true;
            }

            time = default(TimeSpan);
            return false;
        }

        private bool TryParseTimeNormal(string input, out TimeSpan time)
        {
            if (TimeSpan.TryParseExact(input, new[]{ @"d\ h\:mm", @"d\ h", @"d\d\ h\h" },
                CultureInfo.InvariantCulture, out TimeSpan time1))
            {
                time = time1;
                return true;
            }

            time = default(TimeSpan);
            return false;
        }

        private bool TryParseTimeMoreThan24Hours(string input, out TimeSpan time)
        {
            var match = _timeRegex.Match(input);
            if (match.Success)
            {
                var hours = int.Parse(match.Groups["hours"].Value.Trim());
                var minutes = int.Parse(match.Groups["minutes"].Value.Trim());
                time = new TimeSpan(hours, minutes, 0);
                return true;
            }

            time = default(TimeSpan);
            return false;
        }
        
        public TimeExpression Parse(string input)
        {
            var match = _expressionRegex.Match(input);
            if (!match.Success)
            {
                throw new InvalidOperationException();
            }

            var sign = match.Groups["sign"].Value.Trim();
            var left = match.Groups["left"].Value.Trim();
            var right = match.Groups["right"].Value.Trim();

            var timeExpr = new TimeExpression { Sign = sign };

            if (TryParseDate(left, out DateTime leftDate))
            {
                timeExpr.LeftOperand = leftDate;
            }
            else if (TryParseTime(left, out TimeSpan leftTime))
            {
                timeExpr.LeftOperand = leftTime;
            }
            if (TryParseDate(right, out DateTime rightDate))
            {
                timeExpr.RightOperand = rightDate;
            }
            else if (TryParseTime(right, out TimeSpan rightTime))
            {
                timeExpr.RightOperand = rightTime;
            }

            return timeExpr;
        }
    }
}