using System.Collections.Generic;

namespace Pair
{
	public class Calculatable
	{
		public Function function;
		public List<Calculatable> arguments;

		public Object result;

		public Calculatable(Function function, List<Calculatable> arguments) {
			this.function = function;
			this.arguments = arguments;
		}

		public Calculatable(Object result) {
			this.result = result;
		}

		public Object Calculate(bool explain = false) {
			//Debug.LogFormat("{0} = ?", this);
			if (result == null) {
				if (explain) {
					Debug.Log(this);
				}
				result = function.Call(explain, arguments.ToArray());
			}
			//Debug.LogFormat("{0} = {1}", this, PairObject.ToString(result));
			return result;
		}

		public override string ToString() {
			if (function != null) {
				return string.Format(
					"{0}{1}", 
					function.name, 
					arguments.Count > 0 ? "{0}".i(arguments.ExtToString(delimiter: " ", format: " {0}")) : ""
				);
			}
			return result.ToString();
		}
	}
}