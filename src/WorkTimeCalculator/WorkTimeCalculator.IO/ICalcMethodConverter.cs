using System.Reflection;

namespace WorkTimeCalculator.IO
{
    public interface ICalcMethodConverter
    {
        MethodInfo ConvertToCalcMethod(TimeExpression timeExpr);
    }
}