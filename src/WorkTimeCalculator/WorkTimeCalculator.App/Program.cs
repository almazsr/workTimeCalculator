using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WorkTimeCalculator.App
{
	public class DateOrTimeParser
	{
		private readonly Regex _timeRegex;

		public DateOrTimeParser()
		{
			_timeRegex = new Regex("(?<hours>0?[0-9]|1[0-9]|2[0-3]):(?<minutes>[0-5][0-9])", RegexOptions.Compiled);
		}

		private bool TryParseTime(string input, out TimeSpan time)
		{
			var match = _timeRegex.Match(input);
			if(match.Success)
			{
				var hours = int.Parse(match.Groups["hours"].Value);
				var minutes = int.Parse(match.Groups["minutes"].Value);
				time = new TimeSpan(hours, minutes, 0);
				return true;
			}

			time = default(TimeSpan);
			return false;
		}

		private bool TryParseDate(string input, out DateTime date)
		{
			if(DateTime.TryParseExact(input, "dd.MM.yy HH:mm",
				CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date1))
			{
				date = date1;
				return true;
			}

			date = default(DateTime);
			return false;
		}

		public DateOrTime Parse(string input)
		{
			var dateOrTime = new DateOrTime();
			if (TryParseDate(input, out DateTime date))
			{
				dateOrTime.Date = date;
			}
			if (TryParseTime(input, out TimeSpan time))
			{
				dateOrTime.Time = time;
			}

			return dateOrTime;
		}
	}

	public struct DateOrTime
	{
		public DateTime Date;
		public TimeSpan Time;
		public bool HasDate => Date != default(DateTime);
		public bool HasTime => Time != default(TimeSpan);
		public bool HasAny => HasDate || HasTime;
	}

	public class CalculationMethod
	{
		public string Name { get; set; }
		public Type[] ParameterTypes { get; set; }
	}

	public class TimeExpressionParser
	{
		private readonly DateOrTimeParser _parser;
		private readonly Regex _expressionRegex;

		public TimeExpressionParser(DateOrTimeParser parser)
		{
			_parser = parser;
			_expressionRegex = new Regex("(?<left>.+)(?<sign>[+-]{1})(?<right>.+)", RegexOptions.Compiled);
		}

		private CalculationMethod GetCalculationMethod(string sign, DateOrTime left, DateOrTime right)
		{
			if (!left.HasAny || !right.HasAny)
			{
				throw new InvalidOperationException();
			}

			if (sign == "+")
			{
				if (left.HasDate && right.HasTime)
				{
					
				}
				return left.HasDate && right.HasTime || left.HasTime && right.HasDate;
			}

			if (sign == "-")
			{
				return left.HasDate && right.HasTime || left.HasDate && right.HasDate;
			}

			return false;
		}

		public void Parse(string input)
		{
			var match = _expressionRegex.Match(input);
			if (match.Success)
			{
				var sign = match.Groups["sign"].Value;
				var left = match.Groups["left"].Value;
				var right = match.Groups["right"].Value;
				var leftDateOrTime = _parser.Parse(left);
				var rightDateOrTime = _parser.Parse(right);
				if (IsExpressionValid(sign, leftDateOrTime, rightDateOrTime))
				{

				}
			}
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			var cmd = Environment.CommandLine;

			var sign = "?";
			if (cmd.Contains("+"))
			{
				sign = "+";
			}
			if (cmd.Contains("-"))
			{
				sign = "-";
			}

			var cmdSplits = Environment.CommandLine.Split(new []{"-", "+"}, StringSplitOptions.RemoveEmptyEntries);
			var left = cmdSplits[0].Trim();
			var right = cmdSplits[1].Trim();



			//var s =
			//	$"WorkTimeCalculator(new SimpleWorkDayCalendar(), TimeSpan.FromHours({workDayStart}), TimeSpan.FromHours({workDayEnd}));";
			//CodeCompiler codeCompiler = codeDomProvider.CompileAssemblyFromSource(compilerParameters, )
			var exprParser = new TimeExpressionParser();
			var workTimeCalculator = new WorkTimeCalculator(new SimpleWorkDayCalendar(), TimeSpan.FromHours(10), TimeSpan.FromHours(18));
			var result = exprParser.Parse(typeof(WorkTimeCalculator), "").Evaluate(workTimeCalculator);
			Console.WriteLine(result);
		}
	}
}
