using System.Collections.Generic;

namespace Pair
{
	public abstract class Expression
	{
		public abstract Calculatable Evaluate(
			IArgumentable argumentable = null,
			bool explain = false, 
			params Calculatable[] argumentValues
		);

		public abstract Expression Substitute(
			IArgumentable argumentable,
			bool explain = false, 
			params Expression[] argumentValues
		);
	}
}