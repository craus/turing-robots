using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace PairConsole
{
	static class Interactor
	{
		public static void Run() {
			Pair.Program program = new Pair.Program();
			while (true) {
				Console.Write("> ");
				string command = Console.ReadLine();
				if (command == "q") {
					break;
				}
				Console.WriteLine(command);
			}
			Console.WriteLine("Interactor end");
		}
	}
}
