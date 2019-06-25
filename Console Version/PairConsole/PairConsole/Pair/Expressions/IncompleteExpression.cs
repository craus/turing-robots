using System.Collections.Generic;

namespace Pair
{
	public class IncompleteExpression : Expression
	{
		public override Calculatable Evaluate(IArgumentable argumentable = null, bool explain = false, params Calculatable[] argumentValues) {
			throw new System.NotImplementedException();
		}

		public override Expression Substitute(IArgumentable argumentable, bool explain = false, params Expression[] argumentValues) {
			throw new System.NotImplementedException();
		}
	}
}