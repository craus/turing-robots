using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace PairConsole
{
    public class Program
    {
		public static Map<string, Stopwatch> selfTime = new Map<string, Stopwatch>(() => new Stopwatch());
		public static Map<string, Stopwatch> totalTime = new Map<string, Stopwatch>(() => new Stopwatch());

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


		static void OutputTime<T>(Map<T, Stopwatch> time, string kind) {
			time.OrderBy(k => k.Value.ElapsedMilliseconds).ToList().ForEach(k => {
				if (k.Value.ElapsedMilliseconds <= 0) {
					return;
				}
				Debug.LogFormat(3, "{0} {2}: {1}", k.Key, k.Value.ElapsedMilliseconds, kind);
			});
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

			if (args.Length == 0) {
				Interactor.Run();
				return;
			} 

			Stopwatch watch = new Stopwatch();
			watch.Start();
			var program = new Pair.Program(args[0]);
			Debug.LogFormat(1, "Compile time: {0}", watch.ElapsedMilliseconds);
			if (program.error != null) {
				return;
			}
			watch.Restart();
			program.Run();
			Debug.LogFormat(1, "Run time: {0}", watch.ElapsedMilliseconds);
			Debug.LogFormat(1, "Max stack: {0}", Pair.Program.maxDepth);

			OutputTime(Pair.Function.selfTime, "selftime");
			OutputTime(Pair.Function.totalTime, "totaltime");

			totalTime.Keys.ToList().ForEach(key => {
				if (totalTime[key].ElapsedMilliseconds <= 0) {
					return;
				}
				Debug.LogFormat(3, "{0} totaltime: {1}", key, totalTime[key].ElapsedMilliseconds);
			});
			//writer.Close();
			//ostrm.Close();
		}
    }
}
