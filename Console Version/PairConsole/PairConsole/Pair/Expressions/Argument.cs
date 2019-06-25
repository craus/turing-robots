using System.Collections.Generic;

namespace Pair
{
	public class Argument : Expression
	{
		public IArgumentable owner;

		public string name;

		public int index;

		public Argument(IArgumentable owner, string name, int index) {
			this.owner = owner;
			this.name = name;
			this.index = index;
		}

		public override Calculatable Evaluate(
			IArgumentable argumentable,
			bool explain = false,
			params Calculatable[] argumentValues
		) {
			return argumentValues[index];
		}

		public override Expression Substitute(
			IArgumentable argumentable,
			bool explain = false,
			params Expression[] argumentValues
		) {
			if (argumentable == owner) {
				return argumentValues[index];
			}
			return this;
		}

		public override string ToString() {
			return name;
		}
	}
}