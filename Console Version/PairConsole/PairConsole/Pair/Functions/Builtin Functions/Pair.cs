using System.Collections.Generic;

namespace Pair
{
	public class Pair : BuiltinFunction
	{
		public Pair() {
			name = "pair";
			arguments.Add(new Argument(this, "x", 0));
			arguments.Add(new Argument(this, "y", 1));
		}

		protected override Object CallInternal(bool explain = false, params Calculatable[] arguments) {
			return new PairObject(arguments[0], arguments[1]);
		}
	}
}