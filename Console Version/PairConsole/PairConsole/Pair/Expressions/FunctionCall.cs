using System.Collections.Generic;
using System.Linq;

namespace Pair
{
	public class FunctionCall : Expression
	{
		public Function function;
		public List<Expression> arguments = new List<Expression>();

		public Calculatable evaluated = null;

		public FunctionCall() {
		}

		public FunctionCall(Function function, List<Expression> arguments) {
			this.function = function;
			this.arguments = arguments;
		}

		public override Calculatable Evaluate(
			IArgumentable argumentable,
			bool explain = false, 
			params Calculatable[] argumentValues
		) {
			if (explain) {
				//Debug.Log(this);
			}
			var newC = new Calculatable(function, arguments.Select(expr => expr.Evaluate(argumentable, explain, argumentValues)).ToList());
			if (!newC.Same(evaluated)) {
				evaluated = newC;
			}
			return evaluated;
		}

		public override Expression Substitute(
			IArgumentable argumentable,
			bool explain = false,
			params Expression[] argumentValues
		) {
			if (Debug.verbosity >= 3) {
				PairConsole.Program.totalTime["Substitute"].Start();
			}
			if (explain) {
				//Debug.Log(this);
			}
			var newFC = new FunctionCall(
				function, 
				arguments.Select(expr => expr.Substitute(
					argumentable, explain, argumentValues)).ToList()
			);
			if (Same(newFC)) {
				if (Debug.verbosity >= 3) {
					PairConsole.Program.totalTime["Substitute"].Stop();
				}
				return this;
			}
			if (Debug.verbosity >= 3) {
				PairConsole.Program.totalTime["Substitute"].Stop();
			}
			return newFC;
		}

		bool Same(FunctionCall fc) {
			for (int i = 0; i < arguments.Count; i++) {
				if (arguments[i] != fc.arguments[i]) {
					return false;
				}
			}
			return true;
		}

		public override IEnumerable<DefinedFunction> Calls() {
			var argsCalls = arguments.SelectMany(a => a.Calls());
			var df = function as DefinedFunction;
			if (df == null) {
				return argsCalls;
			} else {
				return df.Yield().Concat(argsCalls);
			}
		}

		public override Expression Optimize() {
			for (int i = 0; i < arguments.Count; i++) {
				arguments[i] = arguments[i].Optimize();
			}
			var df = function as DefinedFunction;
			if (df == null) {
				return this;
			}
			if (df.Recursive()) {
				return this;
			}
			df.Optimize();
			var result = df.body.Substitute(df, false, arguments.ToArray());
			return result;
		}

		public override string ToString() {
			return "{0}{1}".i(function.name, arguments.Count > 0 ? arguments.ExtToString(delimiter: " ", format: " {0}") : "");
		}
	}
}