using System.Collections.Generic;
namespace Pair
{
	public class Nil : BuiltinFunction
	{
		public Nil() {
			name = "nil";
		}

		protected override PairObject CallInternal(params Calculatable[] arguments) {
			return null;
		}
	}
}