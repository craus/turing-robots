using System.Collections.Generic;

namespace Pair
{
	public class Second : BuiltinFunction
	{
		public Second() {
			name = "second";
			arguments.Add(new Argument("p", 0));
		}

		public static Calculatable Call(Calculatable c) {
			var pair = c.Calculate() as PairObject;
			if (pair == null) {
				return null;
			}
			return pair.second;
		}
	
		protected override Object CallInternal(params Calculatable[] arguments) {
			var pair = arguments[0].Calculate() as PairObject;
			if (pair == null) {
				return null;
			}
			return pair.second.Calculate();
		}
	}
}