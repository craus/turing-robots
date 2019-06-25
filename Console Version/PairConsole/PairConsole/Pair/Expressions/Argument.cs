using System.Collections.Generic;

namespace Pair
{
	public class Argument : Expression
	{
		public string name;

		public int index;

		public Argument(string name, int index) {
			this.name = name;
			this.index = index;
		}

		public override Calculatable Evaluate(bool explain = false, params Calculatable[] argumentValues) {
			return argumentValues[index];
		}

		public override string ToString() {
			return name;
		}
	}
}