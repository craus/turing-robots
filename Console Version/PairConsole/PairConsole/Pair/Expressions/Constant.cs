using System.Collections.Generic;

namespace Pair
{
	public class Constant : Expression
	{
		public Calculatable x;

		public Constant(Object x) {
			this.x = new Calculatable(x);
		}

		public override Calculatable Evaluate(
			IArgumentable argumentable,
			bool explain, 
			List<Calculatable> argumentValues
		) {
			return x;
		}

		public override Expression Substitute<T>(IArgumentable argumentable, bool explain, List<T> argumentValues) {
			return x;
		}

		public override string ToString() {
			return x.ToString();
		}
	}
}