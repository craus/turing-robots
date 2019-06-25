using System.Collections.Generic;

namespace Pair
{
	public class Call : BuiltinFunction
	{
		public Call() {
			name = "call";
			arguments.Add(new Argument("f", 0));
			arguments.Add(new Argument("args", 1));
		}

		List<Calculatable> Args(Calculatable list, int len) {
			List<Calculatable> result = new List<Calculatable>();
			for (int i = 0; i < len; i++) {
				result.Add(First.Call(list));
				list = Second.Call(list);
			}
			return result;
		}

		protected override Object CallInternal(bool explain = false, params Calculatable[] arguments) {
			var f = arguments[0].Calculate(explain) as FunctionObject;
			return f.f.Call(explain, Args(arguments[1], f.f.arguments.Count).ToArray());
		}
	}
}