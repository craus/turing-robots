using System.Collections.Generic;
using System.Linq;

namespace Pair
{
	public class FunctionCall : Expression
	{
		public Function function;
		public List<Expression> arguments = new List<Expression>();

		public FunctionCall() {
		}

		public FunctionCall(Function function, List<Expression> arguments) {
			this.function = function;
			this.arguments = arguments;
		}

		public override Calculatable Evaluate(
			IArgumentable argumentable,
			bool explain, 
			List<Calculatable> argumentValues
		) {
			if (explain) {
				//Debug.Log(this);
			}
			return new Calculatable(function, arguments.Select(expr => expr.Evaluate(argumentable, explain, argumentValues)).ToList());
		}

		public override Expression Substitute<T>(
			IArgumentable argumentable,
			bool explain,
			List<T> argumentValues
		) {
			if (explain) {
				//Debug.Log(this);
			}
			return new FunctionCall(
				function, 
				arguments.Select(expr => expr.Substitute(
					argumentable, explain, argumentValues)).ToList()
			);
		}
		//public 

		public override string ToString() {
			return "{0}{1}".i(function.name, arguments.Count > 0 ? arguments.ExtToString(delimiter: " ", format: " {0}") : "");
		}
	}
}