using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class RuntimeError : PairError
{
	public RuntimeError(string message, Stack<string> trace)
		: base("{0}\n{1}".i(message, trace.ExtToString("\n\t", "\t{0}"))) {
	}
}
