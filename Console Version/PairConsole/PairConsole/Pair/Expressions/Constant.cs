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
			bool explain = false, 
			params Calculatable[] argumentValues
		) {
			return x;
		}

		public override Expression Substitute(IArgumentable argumentable, bool explain = false, params Expression[] argumentValues) {
			return x;
		}

		public override string ToString() {
			return x.ToString();
		}
	}
}