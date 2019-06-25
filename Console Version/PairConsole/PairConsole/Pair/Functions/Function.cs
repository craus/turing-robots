using System.Collections.Generic;
using System.Linq;

namespace Pair
{
	public abstract class Function
	{
		public string name;
		public List<Argument> arguments = new List<Argument>();

		//public Dictionary< hash;

		protected abstract Object CallInternal(params Calculatable[] arguments);

		public Map<string, Expression> Context() {
			var result = new Map<string, Expression>();
			arguments.ForEach(a => result.Add(a.name, a));
			return result;
		}

		public Object Call(params Calculatable[] arguments) {
			var result = CallInternal(arguments);
			return result;
		}
		public override string ToString() {
			return name;
			//return "{0}{1}".i(name, arguments.Count > 0 ? arguments.ExtToString(format: "({0})") : "");
		}
	}
}