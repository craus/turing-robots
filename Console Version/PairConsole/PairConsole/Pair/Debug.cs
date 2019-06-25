using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Debug
{
	public static int verbosity = 0;

	public static void Log(params object[] args) {
		Console.WriteLine(args.Select(arg => arg.ToString()).ExtToString(format: "{0}"));
	}

	public static void LogFormat(string format, params object[] args) {
		Console.WriteLine(format.i(args));
	}

	public static void LogWarning(string format, params object[] args) {
		Console.WriteLine("WARNING: " + format.i(args));
	}
}
