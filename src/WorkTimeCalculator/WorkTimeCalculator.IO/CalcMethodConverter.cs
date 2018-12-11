using System;
using System.Reflection;

namespace WorkTimeCalculator.IO
{
    public class CalcMethodConverter : ICalcMethodConverter
    {
        private readonly Type _calculatorType;

        public CalcMethodConverter(Type calculatorType)
        {
            _calculatorType = calculatorType;
        }

        private string GetCalcMethodName(string sign)
        {
            switch (sign)
            {
                case "+":
                    return "Add";
                case "-":
                    return "Subtract";
                default:
                    throw new ArgumentException("Invalid sign", nameof(sign));
            }
        }

        public MethodInfo ConvertToCalcMethod(TimeExpression timeExpr)
        {
            var methodName = GetCalcMethodName(timeExpr.Sign);
            return _calculatorType.GetRuntimeMethod(methodName,
                new[] {timeExpr.LeftOperand.GetType(), timeExpr.RightOperand.GetType()});
        }
    }
}