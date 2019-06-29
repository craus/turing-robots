using System;
using System.Collections.Generic;

namespace Pair
{
	public class Bin : BuiltinFunction
	{
		public ConsoleIO io;

		public Bin(ConsoleIO io) {
			name = "bin";
			this.io = io;
			arguments.Add(new Argument(this, "callback", 0));
		}

		protected override Object CallInternal(bool explain, List<Calculatable> argumentValues) {
			return new CommandObject(() => {
				Func<CommandObject> nextFunction = null;
				io.Bin(bit => {
					var f = argumentValues[0].Calculate(explain) as FunctionObject;
					if (f != null) {
						nextFunction = () => f.f.Call(explain, bit) as CommandObject;
					}
				});
				return nextFunction;
			});
		}
	}
}