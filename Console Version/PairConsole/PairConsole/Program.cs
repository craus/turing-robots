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

			var program = new Pair.Program(args[0]);
			program.Run();

			//writer.Close();
			//ostrm.Close();
        }
    }
}
