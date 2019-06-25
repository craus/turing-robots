using System.Collections.Generic;

namespace Pair
{
	public class First : BuiltinFunction
	{
		public First() {
			name = "first";
			arguments.Add(new Argument("p", 0));
		}

		public static Calculatable Call(Calculatable c) {
			var pair = c.Calculate() as PairObject;
			if (pair == null) {
				return null;
			}
			return pair.first;
		}

		protected override Object CallInternal(bool explain = false, params Calculatable[] arguments) {
			var pair = arguments[0].Calculate() as PairObject;
			if (pair == null) {
				return null;
			}
			return pair.first.Calculate();
		}
	}
}