using System.Collections.Generic;

namespace Pair
{
	public class If : BuiltinFunction
	{
		public If() {
			name = "if";
			arguments.Add(new Argument("cond", 0));
			arguments.Add(new Argument("when_true", 1));
			arguments.Add(new Argument("when_false", 2));
		}

		protected override Object CallInternal(bool explain = false, params Calculatable[] arguments) {
			if (arguments[0].Calculate(explain) != null) {
				return arguments[1].Calculate(explain);
			} else {
				return arguments[2].Calculate(explain);
			}
		}
	}
}