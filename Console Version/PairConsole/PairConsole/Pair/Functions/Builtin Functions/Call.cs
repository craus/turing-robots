using System.Collections.Generic;

namespace Pair
{
	public class Call : BuiltinFunction
	{
		public Call() {
			name = "call";
			arguments.Add(new Argument(this, "f", 0));
			arguments.Add(new Argument(this, "args", 1));
		}

		List<Calculatable> Args(Calculatable list, int len) {
			List<Calculatable> result = new List<Calculatable>();
			for (int i = 0; i < len; i++) {
				result.Add(First.Call(list));
				list = Second.Call(list);
			}
			return result;
		}

		protected override Object CallInternal(bool explain, List<Calculatable> argumentValues) {
			var f = argumentValues[0].Calculate(explain) as FunctionObject;
			if (f == null) {
				return null;
			}
			return f.f.Call(explain, Args(argumentValues[1], f.f.arguments.Count).ToArray());
		}
	}
}