using System.Collections.Generic;

namespace Pair
{
	public class IncompleteExpression : Expression
	{
		public override Calculatable Evaluate(IArgumentable argumentable, bool explain, List<Calculatable> argumentValues) {
			throw new System.NotImplementedException();
		}

		public override Expression Substitute<T>(
			IArgumentable argumentable, 
			bool explain, 
			List<T> argumentValues
		) {
			throw new System.NotImplementedException();
		}
	}
}