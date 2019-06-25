﻿using System.Collections.Generic;
using System.Linq;
namespace Pair
{
	public class DefinedFunction : Function
	{
		public Expression body;

		protected override Object CallInternal(bool explain = false, params Calculatable[] arguments) {
			var exp = body.Evaluate(explain, arguments);
			var args = arguments.ToList().ExtToString(format: "({0})");
			//Debug.LogFormat("{0}{1} = {2}", name, this.arguments.Count > 0 ? args : "", exp);
			var result = exp.Calculate(explain);
			//Debug.LogFormat("{0}{1} = {2}", name, this.arguments.Count > 0 ? args : "", PairObject.ToString(result));
			return result;
		}

		public override string ToString() {
			return string.Format("{0} {1}", base.ToString(), arguments.ExtToString());
		}
	}
}