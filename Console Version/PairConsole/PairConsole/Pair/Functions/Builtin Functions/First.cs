using System.Collections.Generic;

namespace Pair
{
	public class First : BuiltinFunction
	{
		public First() {
			name = "first";
			arguments.Add(new Argument(this, "p", 0));
		}

		public static Calculatable Call(Calculatable c) {
			var pair = c.Calculate() as PairObject;
			if (pair == null) {
				return new Calculatable(null);
			}
			return pair.first;
		}

		protected override Object CallInternal(bool explain = false, List<Calculatable> argumentValues = null) {
			if (argumentValues.Count < 1) {
				throw new System.Exception("Too few arguments");
			}
			var pair = argumentValues[0].Calculate() as PairObject;
			if (pair == null) {
				return null;
			}
			if (pair.first == null) {
				throw new System.Exception("Pair with first = null");
			}
			return pair.first.Calculate();
		}
	}
}