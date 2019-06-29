using System.Collections.Generic;

namespace Pair
{
	public class If : BuiltinFunction
	{
		public If() {
			name = "if";
			arguments.Add(new Argument(this, "cond", 0));
			arguments.Add(new Argument(this, "when_true", 1));
			arguments.Add(new Argument(this, "when_false", 2));
		}

		protected override Object CallInternal(bool explain = false, List<Calculatable> argumentValues = null) {
			if (argumentValues[0].Calculate(explain) != null) {
				return argumentValues[1].Calculate(explain);
			} else {
				return argumentValues[2].Calculate(explain);
			}
		}
	}
}