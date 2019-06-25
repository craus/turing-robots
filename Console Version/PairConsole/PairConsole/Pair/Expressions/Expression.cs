using System.Collections.Generic;

namespace Pair
{
	public abstract class Expression
	{
		public abstract Calculatable Evaluate(bool explain = false, params Calculatable[] argumentValues);
	}
}