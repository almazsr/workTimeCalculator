using System;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;

namespace WorkTimeCalculator
{
	public class TimeExpressionParser
	{
		public MethodInfo GetMethodInfo(Type calculatorType, string sign)
		{
			switch(sign)
			{
				case "+":
					return calculatorType.GetMethod("Add");
				case "-":
					return calculatorType.GetMethod("Subtract");
				case "/":
					return calculatorType.GetMethod("Divide");
				case "*":
					return calculatorType.GetMethod("Multiply");
				default:
					throw new InvalidOperationException("Invalid sign");
			}
		}

		public TimeExpression Parse(Type calculatorType, string input)
		{
			var match = Regex.Match(input, @"\[(?<left>.+)\](?<sign>[+\-\/*])\[(?<right>.+)\]");
			var sign = match.Groups["sign"].Value;
			
			var calcMethod = GetMethodInfo(calculatorType, sign);

			var calcParameters = calcMethod.GetParameters();
			var leftParameter = calcParameters[0];
			var rightParameter = calcParameters[1];
			var left = TypeDescriptor.GetConverter(leftParameter.ParameterType).ConvertFromString(match.Groups["left"].Value);
			var right = TypeDescriptor.GetConverter(rightParameter.ParameterType).ConvertFromString(match.Groups["right"].Value);
			return new TimeExpression {Method = calcMethod, LeftOperand = left, RightOperand = right};
		}
	}
}