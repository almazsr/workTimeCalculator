namespace WorkTimeCalculator.IO
{
    public interface ITimeCalculatorParser
    {
        ITimeCalculator Parse(string input);
    }
}