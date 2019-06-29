using System.Collections.Generic;

namespace Pair
{
	public class IncompleteExpression : Expression
	{
		public override Calculatable Evaluate(IArgumentable argumentable, bool explain, List<Calculatable> argumentValues) {
			throw new System.NotImplementedException();
		}

		public override Expression Substitute(IArgumentable argumentable, bool explain, List<Expression> argumentValues) {
			throw new System.NotImplementedException();
		}
	}
}