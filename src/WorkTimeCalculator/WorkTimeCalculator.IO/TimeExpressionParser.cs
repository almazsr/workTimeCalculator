using System;
using System.Text.RegularExpressions;

namespace WorkTimeCalculator.IO
{
	public class TimeExpressionParser : ITimeExpressionParser
    {
        private readonly Regex _expressionRegex;
	    private readonly ITimeArgumentParser _timeArgumentParser;
        
        public TimeExpressionParser(ITimeArgumentParser timeArgumentParser)
        {
	        _timeArgumentParser = timeArgumentParser;
	        _expressionRegex = new Regex(@"(?<calculator>[Ww]|[Tt]|) ?(?<left>.+)(?<sign>[+-]{1})(?<right>.+)", RegexOptions.Compiled);
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

	        return new TimeExpression
	        {
		        Sign = sign,
		        LeftOperand = _timeArgumentParser.Parse(left),
				RightOperand = _timeArgumentParser.Parse(right)
	        };
        }
    }
}