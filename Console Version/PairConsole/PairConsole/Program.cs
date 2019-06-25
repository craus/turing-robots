using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PairConsole
{
    class Program
    {
        static string PROGRAM_FILE = "Pair Code/program.pair";
        static string OUTPUT_FILE = "Pair Code/output.txt";

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
			Debug.Log("/*".StartsWith("/*"));
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

			var program = new Pair.Program(args[0]);
			program.Run();

			while (Console.ReadLine() == "") {
			}
			//writer.Close();
			//ostrm.Close();
		}
    }
}
