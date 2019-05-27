using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CompileError : Exception
{
	public CompileError(string message, CodePosition position, Stack<string> trace)
		: base("{0}: {1}\n{2}".i(position, message, trace.ExtToString("\n\t", "\t{0}"))) {
	}
}
