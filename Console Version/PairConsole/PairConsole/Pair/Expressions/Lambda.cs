using System.Collections.Generic;
using System.Linq;

namespace Pair
{
	public class Lambda : Expression, IArgumentable
	{
		public List<Argument> arguments = new List<Argument>();
		public Expression body;

		public IArgumentable parent;

		public Lambda() {
			parent = this;
		}

		public Lambda(IArgumentable parent, List<Argument> arguments, Expression body) {
			this.arguments = arguments;
			this.body = body;
			this.parent = parent;
		}

		public Map<string, Expression> Context() {
			var result = new Map<string, Expression>();
			arguments.ForEach(a => result.Add(a.name, a));
			return result;
		}

		public override Calculatable Evaluate(
			IArgumentable argumentable,
			bool explain = false, 
			params Calculatable[] argumentValues
		) {
			var f = new DefinedFunction();
			f.arguments = arguments;
			f.name = "<lambda>";
			f.body = body.Substitute(argumentable, explain, argumentValues);
			f.parent = parent;
			return new Calculatable(new FunctionObject(f));
		}

		public override Expression Substitute(
			IArgumentable argumentable,
			bool explain = false,
			params Expression[] argumentValues
		) {
			return new Lambda(parent, arguments, body.Substitute(
				argumentable,
				explain,
				argumentValues
			));
		}

		public override string ToString() {
			return "function{0} to {1}".i(
				arguments.Count > 0 ? arguments.ExtToString(delimiter: " ", format: " {0}") : "",
				body
			);
		}
	}
}