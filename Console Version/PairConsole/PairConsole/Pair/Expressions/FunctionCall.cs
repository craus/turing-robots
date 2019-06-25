using System.Collections.Generic;
using System.Linq;

namespace Pair
{
	public class FunctionCall : Expression
	{
		public Function function;
		public List<Expression> arguments = new List<Expression>();

		public override Calculatable Evaluate(bool explain = false, params Calculatable[] argumentValues) {
			if (explain) {
				//Debug.Log(this);
			}
			return new Calculatable(function, arguments.Select(expr => expr.Evaluate(explain, argumentValues)).ToList());
		}

		public override string ToString() {
			return "{0}{1}".i(function.name, arguments.Count > 0 ? arguments.ExtToString(delimiter: " ", format: " {0}") : "");
		}
	}
}