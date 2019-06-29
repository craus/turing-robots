using System.Collections.Generic;
using System.Linq;
namespace Pair
{
	public class DefinedFunction : Function, IArgumentable
	{
		public CodePosition position;

		public Expression body;

		public IArgumentable parent = null;

		public DefinedFunction() {
			parent = this;
		}

		protected override Object CallInternal(bool explain, List<Calculatable> argumentValues) {
			var exp = body.Evaluate(parent, explain, argumentValues);
			if (explain) {
				var args = argumentValues.ToList().ExtToString(format: "({0})");
			}
			//Debug.LogFormat("{0}{1} = {2}", name, this.arguments.Count > 0 ? args : "", exp);
			var result = exp.Calculate(explain);
			//Debug.LogFormat("{0}{1} = {2}", name, this.arguments.Count > 0 ? args : "", PairObject.ToString(result));
			return result;
		}

		public override string ToString() {
			return string.Format("{0} {1}", base.ToString(), arguments.ExtToString());
		}

		public string Source() {
			return string.Format(
				"{0}{1} is {2}", 
				name, 
				arguments.Count == 0 ? "" : arguments.ExtToString(format: " {0}", delimiter: " "), 
				body
			);
		}
	}
}