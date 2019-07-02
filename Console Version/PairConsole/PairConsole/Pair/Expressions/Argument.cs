using System;
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
			bool explain,
			List<Calculatable> argumentValues
		) {
			if (argumentable == owner) {
				return argumentValues[index];
			}
			throw new Exception("Cannot evaluate argument");
		}

		public override Expression Substitute<T>(
			IArgumentable argumentable,
			bool explain,
			List<T> argumentValues
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