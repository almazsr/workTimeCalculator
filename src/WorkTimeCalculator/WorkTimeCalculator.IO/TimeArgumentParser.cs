using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace WorkTimeCalculator.IO
{
	public class TimeArgumentParser : ITimeArgumentParser
	{
		private readonly Regex[] _workTimeRegexes;

		public TimeArgumentParser()
		{
			_workTimeRegexes = new []
			{
				new Regex(@"(?<days>\d+ |)(?<hours>\d+):(?<minutes>[0-5][0-9])", RegexOptions.Compiled),
				new Regex(@"(?<days>\d+d |)(?<hours>\d+h |)(?<minutes>[0-5][0-9]m|)", RegexOptions.Compiled)
			};
		}

		private bool TryParseDate(string input, out DateTime date)
		{
			if(DateTime.TryParseExact(input, new[] { "dd.MM.yy HH:mm", "dd.MM.yyyy HH:mm", "dd.MM.yy" },
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
			if(TimeSpan.TryParseExact(input, new[] { @"d\ h\:mm", @"d\ h", @"d\d\ h\h" },
				CultureInfo.InvariantCulture, out TimeSpan time1))
			{
				time = time1;
				return true;
			}

			time = default(TimeSpan);
			return false;
		}

		private bool TryParseWorkTime(string input, Regex regex, out WorkTime time)
		{
			var match = regex.Match(input);
			if(match.Success)
			{
				var daysParsed = int.TryParse(match.Groups["days"].Value.Trim(), out int days);
				var hoursParsed = int.TryParse(match.Groups["hours"].Value.Trim(), out int hours);
				var minutesParsed = int.TryParse(match.Groups["minutes"].Value.Trim(), out int minutes);
				if (!daysParsed && !hoursParsed && !minutesParsed) throw new InvalidOperationException();
				time = new WorkTime(days, hours, minutes);
				return true;
			}

			time = default(WorkTime);
			return false;
		}

		private bool TryParseWorkTime(string input, out WorkTime time)
		{
			foreach (var regex in _workTimeRegexes)
			{
				if (TryParseWorkTime(input, regex, out time))
				{
					return true;
				}
			}

			time = default(WorkTime);
			return false;
		}

		public object Parse(string input)
		{
			if(TryParseDate(input, out DateTime leftDate))
			{
				return leftDate;
			}
			if(TryParseTime(input, out TimeSpan leftTime))
			{
				return leftTime;
			}
			if(TryParseWorkTime(input, out WorkTime leftWorkTime))
			{
				return leftWorkTime;
			}

			throw new InvalidOperationException();
		}
	}
}