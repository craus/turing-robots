﻿using System.Collections.Generic;
using System.Linq;

namespace Pair
{
	public abstract class Function : IArgumentable
	{
		public string name;
		public List<Argument> arguments = new List<Argument>();

		public static int depth = 0;
		public static Stack<string> stack = new Stack<string>();
		//public Dictionary< hash;

		protected abstract Object CallInternal(bool explain, List<Calculatable> argumentValues);

		public Map<string, Expression> Context() {
			var result = new Map<string, Expression>();
			arguments.ForEach(a => result.Add(a.name, a));
			return result;
		}

		public Object Call(bool explain = false, params Calculatable[] argumentValues) {
			return Call(explain, argumentValues.ToList());
		}

		public Object Call(bool explain, List<Calculatable> argumentValues) {
			if (stack.Count > Program.MAX_DEPTH) {
				throw new RuntimeError("Stack Overflow", stack);
			}
			stack.Push(name);
			Program.callCnt++;
			if (stack.Count > Program.maxDepth) {
				Program.maxDepth = stack.Count;
			}
			if (explain) {
				//Debug.LogFormat("{0} {1}", name, arguments.ExtToString(delimiter: " ", format: "{0}"));
			}
			var result = CallInternal(explain, argumentValues);
			stack.Pop();
			return result;
		}

		public override string ToString() {
			return name;
			//return "{0}{1}".i(name, arguments.Count > 0 ? arguments.ExtToString(format: "({0})") : "");
		}
	}
}