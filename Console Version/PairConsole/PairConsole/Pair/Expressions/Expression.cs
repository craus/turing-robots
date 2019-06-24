using System.Collections.Generic;

namespace Pair
{
	public abstract class Expression
	{
		public abstract Calculatable Evaluate(params Calculatable[] argumentValues);
	}
}