using System.Collections.Generic;
using System.Linq;

namespace Pair
{
	public abstract class Expression
	{
		public Calculatable Evaluate(
			IArgumentable argumentable = null,
			bool explain = false,
			params Calculatable[] argumentValues
		) {
			return Evaluate(argumentable, explain, argumentValues.ToList());
		}
		
		public abstract Calculatable Evaluate(
			IArgumentable argumentable,
			bool explain, 
			List<Calculatable> argumentValues
		);

		public abstract Expression Substitute<T>(
			IArgumentable argumentable,
			bool explain,
			List<T> argumentValues
		) where T : Expression;
	}
}