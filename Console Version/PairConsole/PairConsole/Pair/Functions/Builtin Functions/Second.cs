using System.Collections.Generic;

namespace Pair
{
	public class Second : BuiltinFunction
	{
		public Second() {
			name = "second";
			arguments.Add(new Argument("p", 0));
		}

		protected override PairObject CallInternal(params Calculatable[] arguments) {
			var pair = arguments[0].Calculate();
			if (pair == null) {
				return null;
			}
			return pair.second;
		}
	}
}