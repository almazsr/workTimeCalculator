﻿using System;
using WorkTimeCalculator.IO;

namespace WorkTimeCalculator.App
{
    internal static class Program
	{
		static void Main(string[] args)
		{
			try
			{
				var cmd = string.Join(" ", args);
				var workDayCalendar = new TatarstanWorkDayCalendar(TimeSpan.FromHours(10), TimeSpan.FromHours(18));
				workDayCalendar.EnsureInitializedAsync().Wait();
				var calculatorParser = new TimeCalculatorParser(workDayCalendar);
				var calculator = calculatorParser.Parse(cmd);
				var timeExpressionParser = new TimeExpressionParser(new TimeArgumentParser());
				var timeExpr = timeExpressionParser.Parse(cmd);
				var calcMethodConverter = new CalcMethodConverter(calculator.GetType());
				var calcMethod = calcMethodConverter.ConvertToCalcMethod(timeExpr);
				var result = calcMethod.Invoke(calculator, new[] { timeExpr.LeftOperand, timeExpr.RightOperand });
				var objFormatter = new ObjectFormatter();
				var output = objFormatter.FormatObject(result);
				Console.WriteLine(output);
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}
}
