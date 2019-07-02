using System.Collections.Generic;
using System.Linq;
namespace Pair
{
	public class DefinedFunction : Function, IArgumentable
	{
		public CodePosition position;

		public Expression body;

		public IArgumentable parent = null;

		public IEnumerable<DefinedFunction> calls;
		public List<DefinedFunction> isCalledBy = new List<DefinedFunction>();

		public IEnumerable<DefinedFunction> IsCalledBy() {
			return isCalledBy;
		}

		public DefinedFunction() {
			parent = this;
		}

		protected override Object CallInternal(bool explain = false, params Calculatable[] argumentValues) {
			var exp = body.Evaluate(parent, explain, argumentValues);
			//if (explain) {
			//	var args = argumentValues.ToList().ExtToString(format: "({0})");
			//}
			//Debug.LogFormat("{0}{1} = {2}", name, this.arguments.Count > 0 ? args : "", exp);
			var result = exp.Calculate(explain);
			//Debug.LogFormat("{0}{1} = {2}", name, this.arguments.Count > 0 ? args : "", PairObject.ToString(result));
			return result;
		}

		bool? recursive;
		public override bool Recursive() {
			if (recursive == null) {
			}
			return recursive ?? false;
		}

		bool optimized;
		bool optimizing;
		public override void Optimize() {
			if (optimized) {
				return;
			}
			if (optimizing) {
				Debug.LogWarning(0, "OPTIMIZATION LOOP");
			}
			optimizing = true;
			if (body == null) {
				return;
			}
			var old = body.ToString();
			var newBody = body.Optimize();
			if (old != newBody.ToString()) {
				Debug.LogFormat(2, "Optimized {0} to {1}\nwas: {2}", this, newBody, old);
			}
			optimized = true;
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