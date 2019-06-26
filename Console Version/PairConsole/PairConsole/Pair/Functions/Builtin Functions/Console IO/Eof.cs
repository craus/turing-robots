using System;
using System.Collections.Generic;

namespace Pair
{
	public class Eof : BuiltinFunction
	{
		public ConsoleIO io;

		public Eof(ConsoleIO io) {
			name = "bin";
			this.io = io;
			arguments.Add(new Argument(this, "callback", 0));
		}

		protected override Object CallInternal(bool explain = false, params Calculatable[] argumentValues) {
			return new CommandObject(() => {
				Func<CommandObject> nextFunction = null;
				io.Eof(eof => {
					var f = argumentValues[0].Calculate(explain) as FunctionObject;
					if (f != null) {
						nextFunction = () => f.f.Call(explain, eof) as CommandObject;
					}
				});
				return nextFunction;
			});
		}
	}
}