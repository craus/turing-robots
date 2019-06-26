using System;
using System.Collections.Generic;

namespace Pair
{
	public class Bout : BuiltinFunction
	{
		public ConsoleIO io;

		public Bout(ConsoleIO io) {
			name = "bout";
			this.io = io;
			arguments.Add(new Argument(this, "bit", 0));
			arguments.Add(new Argument(this, "callback", 1));
		}

		protected override Object CallInternal(bool explain = false, params Calculatable[] argumentValues) {
			return new CommandObject(() => {
				var bit = argumentValues[0].Calculate(explain) as PairObject;
				Func<CommandObject> nextFunction = null;
				io.Bout(bit != null, () => {
					var f = argumentValues[1].Calculate(explain) as FunctionObject;
					if (f != null) {
						nextFunction = () => f.f.Call(explain) as CommandObject;
					}
				});
				return nextFunction;
			});
		}
	}
}