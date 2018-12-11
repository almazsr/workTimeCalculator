using System.Linq.Expressions;
using System.Reflection;

namespace WorkTimeCalculator
{
	public class TimeExpression
	{
		public MethodInfo Method { get; set; }
		public object LeftOperand { get; set; }
		public object RightOperand { get; set; }
		public object Evaluate(object calculator) => Method.Invoke(calculator, new[] {LeftOperand, RightOperand});
	}
}
