using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Debug
{
	public static void Log(params object[] args) {
		Console.WriteLine(args.Select(arg => arg.ToString()).ExtToString());
	}
	
    public static void LogFormat(string format, params object[] args) {
        Console.WriteLine(format.i(args));
    }
}
