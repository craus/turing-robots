﻿using System.Collections.Generic;

namespace Pair
{
	public class Pair : BuiltinFunction
	{
		public Pair() {
			name = "pair";
			arguments.Add(new Argument("x", 0));
			arguments.Add(new Argument("y", 1));
		}

		protected override Object CallInternal(params Calculatable[] arguments) {
			return new PairObject(arguments[0], arguments[1]);
		}
	}
}