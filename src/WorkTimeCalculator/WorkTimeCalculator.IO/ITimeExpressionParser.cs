namespace WorkTimeCalculator.IO
{
    public interface ITimeExpressionParser
    {
        TimeExpression Parse(string input);
    }
}