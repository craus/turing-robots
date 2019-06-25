using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace PairConsole
{
    class Program
    {
		static void TestMapMerge() {

			Map<int, int> a = new Map<int, int>();
			Map<int, int> b = new Map<int, int>();
			a[3] = 5;
			a[4] = 7;
			a[5] = 11;
			b[4] = 8;
			b[6] = 12;
			Debug.LogFormat("a: {0}", a.ExtToString());
			Debug.LogFormat("b: {0}", b.ExtToString());
			Debug.LogFormat("a.merge(b): {0}", a.Merge(b).ExtToString());
			Debug.LogFormat("b.merge(a): {0}", b.Merge(a).ExtToString());
			Debug.LogFormat("a: {0}", a.ExtToString());
			Debug.LogFormat("b: {0}", b.ExtToString());
		}

		static void TestStartsWith() {
			Debug.Log("/*".StartsWith("/*", StringComparison.Ordinal));
		}

        static void Main(string[] args) {
			//FileStream ostrm;
			//StreamWriter writer;
			//TextWriter oldOut = Console.Out;
			//try {
			//    ostrm = new FileStream(OUTPUT_FILE, FileMode.Create, FileAccess.Write);
			//    writer = new StreamWriter(ostrm);
			//} catch (Exception e) {
			//    Console.WriteLine("Cannot open Redirect.txt for writing");
			//    Console.WriteLine(e.Message);
			//    return;
			//}
			//Console.SetOut(writer);

			//TestStartsWith();

			if (args.Length >= 2) {
				Debug.verbosity = int.Parse(args[1]);
			}
			

			Stopwatch watch = new Stopwatch();
			watch.Start();
			var program = new Pair.Program(args[0]);
			Debug.LogFormat("Compile time: {0}", watch.ElapsedMilliseconds);
			watch.Restart();
			program.Run();
			Debug.LogFormat("Run time: {0}", watch.ElapsedMilliseconds);
			Debug.LogFormat("Max stack: {0}", Pair.Program.maxDepth);
			//writer.Close();
			//ostrm.Close();
		}
    }
}
