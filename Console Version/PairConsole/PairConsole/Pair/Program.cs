using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEngine;
#endif
using System.Linq;
using System;
using System.IO;

namespace Pair
{
	public class Program
	{
		public const int MAX_DEPTH = 100000;
		public static int maxDepth = 0;
		public static int callCnt = 0;

		const string DEF = "def";
		const string IS = "is";
		const string ASSERT = "assert";
		const string EXPLAIN = "explain";
		const string OUTPUT = "output";
		const string USE = "use";
		const string UNCALL = "uncall";
		const string FUNCTION = "function";
		const string TO = "to";
		const string MAIN = "main";

		public Map<string, Function> functions = new Map<string, Function>();
		public List<Expression> asserts = new List<Expression>();
		public List<Expression> outputs = new List<Expression>();

		public List<Expression> explain = new List<Expression>();

		public List<Token> tokens = new List<Token>();
		public int cur;

		public HashSet<string> files = new HashSet<string>();

		public PairError error;

		public Stack<string> trace = new Stack<string>();

		public DefinedFunction last;

		Func<CommandObject> currentFunction = null;

		ConsoleIO io = new ConsoleIO();


		void Move(int delta) {
			cur += delta;
		}

		Token CurToken() {
			if (cur >= tokens.Count) {
				throw new CompileError("Unexpected end of program", tokens.Last().position, trace);
			}
			return tokens[cur];
		}

		Lambda ReadLambda(Map<string, Expression> context = null) {
			if (context == null) {
				context = new Map<string, Expression>();
			}
			trace.Push("read lambda {0}{1}".i(
				CurToken(),
				functions.ContainsKey(CurToken())
					? " ({0} args)".i(functions[CurToken()].arguments.Count)
					: ""
			));
			var lambda = new Lambda();
			lambda.arguments = new List<Argument>();
			//Debug.LogFormat("Defining function: {0}", f.name);
			Move(1);
			while (CurToken() != TO) {
				lambda.arguments.Add(new Argument(lambda, CurToken(), lambda.arguments.Count()));
				Move(1);
			}

			Move(1);// skip "to"	
			lambda.body = ReadExpression(lambda.Context().Merge(context));
			//Debug.LogFormat("{0} body end", f.name);
			trace.Pop();
			return lambda;
		}


		Expression ReadExpression(Map<string, Expression> context = null) {
			if (context == null) {
				context = new Map<string, Expression>();
			}
			trace.Push("read expression {0}{1}".i(
				CurToken(),
				functions.ContainsKey(CurToken())
					? " ({0} args)".i(functions[CurToken()].arguments.Count)
					: ""
			));
			if (CurToken() == UNCALL) {
				Move(1);
				if (!functions.ContainsKey(CurToken())) {
					//Debug.LogFormat("Functions: {0}", functions.ExtToString());
					throw new CompileError("No such function: '{0}'".i(CurToken().text), CurToken().position, trace);
				}
				var f = new Constant(new FunctionObject(functions[CurToken()]));
				Move(1);
				trace.Pop();
				return f;
			}
			if (CurToken() == FUNCTION) {
				var lambda = ReadLambda(context);
				trace.Pop();
				return lambda;
			}
			if (context[CurToken()] != null) {
				var result = context[CurToken()];
				Move(1);
				trace.Pop();
				return result;
			}
			var exp = new FunctionCall();
			if (!functions.ContainsKey(CurToken())) {
				//Debug.LogFormat("Functions: {0}", functions.ExtToString());
				throw new CompileError("No such function: '{0}'".i(CurToken().text), CurToken().position, trace);
			}
			exp.function = functions[CurToken()];
			var df = exp.function as DefinedFunction;
			if (df != null) {
				if (df.body == null) {
					Debug.LogWarning(1,
						"{3} Function definition deduced: {0} [{2} args] ({1})",
						df.name,
						df.position,
						df.arguments.Count,
						CurToken().position
					);
				}
			}
			//Debug.LogFormat("Reading function call: {0}", exp.function.name);
			Move(1);
			for (int i = 0; i < exp.function.arguments.Count; i++) {
				exp.arguments.Add(ReadExpression(context));
			}
			trace.Pop();
			return exp;
		}

		void FindFunctionDefinitions() {
			var shortestIs = new Map<string, Tuple<int, Token>>(
				() => new Tuple<int, Token>(int.MaxValue, null)
			);
			for (int i = 0; i < tokens.Count; i++) {
				if (tokens[i] == IS) {
					for (int j = 1; j <= i; j++) {
						if (tokens[i - j] == IS) {
							break;
						}
						if (i-j-1 >= 0 && tokens[i - j - 1] == USE) {
							continue;
						}
						if (j < shortestIs[tokens[i - j]].Item1) {
							shortestIs[tokens[i - j]] = new Tuple<int, Token>(j, tokens[i - j]);
						}
					}
				}
			}
			shortestIs.Keys.ToList().ForEach(key => {
				if (functions.ContainsKey(key)) {
					return;
				}
				if (key == ASSERT) {
					return;
				}
				functions.Add(key, new DefinedFunction());
				functions[key].name = key;
				(functions[key] as DefinedFunction).position = shortestIs[key].Item2.position;
				for (int i = 0; i < shortestIs[key].Item1 - 1; i++) {
					functions[key].arguments.Add(new Argument(functions[key], "", i));
				}
			});
		}

		void ReadFunctionDefinition() {
			trace.Push("read function definition {0}".i(CurToken()));
			var f = functions[CurToken()] as DefinedFunction;
			if (f == null) {
				throw new CompileError("Trying to define function: '{0}'".i(CurToken().text), CurToken().position, trace);
			}
			if (f.body != null) {
				//Debug.LogFormat("Functions: {0}", functions.ExtToString());
				throw new CompileError("Function already exists: '{0} ({1})'".i(f.name, f.position), CurToken().position, trace);
			}
			f.name = CurToken();
			f.position = CurToken().position;

			f.arguments = new List<Argument>();
			//Debug.LogFormat("Defining function: {0}", f.name);
			Move(1);
			while (CurToken() != IS) {
				if (f.arguments.Any(a => a.name == CurToken())) {
					throw new CompileError(
						"Function {0} takes same argument twice: {1}".i(
							"{0} ({1})".i(f.name, f.position),
							CurToken().text
						),
						CurToken().position,
						trace
					);
				}
				f.arguments.Add(new Argument(f, CurToken(), f.arguments.Count()));
				Move(1);
			}
			Move(1);// skip "is"	
			f.body = new IncompleteExpression();
			f.body = ReadExpression(f.Context());
			//Debug.LogFormat("{0} body end", f.name);
			last = f;
			trace.Pop();
		}

		void ReadAssert(List<Expression> list = null) {
			if (list == null) {
				list = asserts;
			}
			Move(1);// skip "assert"	
			list.Add(ReadExpression());
		}

		void ReadOutput() {
			Move(1);// skip "output"	
			outputs.Add(ReadExpression());
		}

		void ReadUse() {
			Move(2);// skip "use" and filename	
		}

		private void ReadProgram() {
			cur = 0;
			while (cur < tokens.Count) {
				if (CurToken() == ASSERT) {
					ReadAssert();
				} else if (CurToken() == EXPLAIN) {
					ReadAssert(explain);
				} else if (CurToken() == OUTPUT) {
					ReadOutput();
				} else if (CurToken() == USE) {
					ReadUse();
				} else {
					ReadFunctionDefinition();
				}
			}
		}

		private void CreateBuiltinFunctions() {
			functions.Add("pair", new Pair());
			functions.Add("first", new First());
			functions.Add("second", new Second());
			functions.Add("nil", new Nil());
			functions.Add("if", new If());
			functions.Add("call", new Call());

			functions.Add("bin", new Bin(io));
			functions.Add("bout", new Bout(io));
			functions.Add("eof", new Eof(io));
		}

		string GetFileName(string current, string relative) {
			var currentDirectory = Path.GetDirectoryName(current);
			var file = Path.Combine(currentDirectory, relative);
			if (File.Exists(file)) {
				return file;
			}
			if (Path.GetExtension(file) == "") {
				file = Path.ChangeExtension(file, "pair");
			}
			if (File.Exists(file)) {
				return file;
			}
			return null;
		}

		bool ignoreMode = false;
		bool ignoreToEndOfLine = false;
		void ReadToken(List<Token> result, Token token) {
			if (ignoreToEndOfLine) {
				return;
			}
			if (token.text.StartsWith("/*", StringComparison.Ordinal)) {
				ignoreMode = true;
			}
			if (!ignoreMode) {
				if (token.text[0] == '#') {
					ignoreToEndOfLine = true;
					return;
				}
				result.Add(token);
			}
			if (token.text.EndsWith("*/", StringComparison.Ordinal)) {
				ignoreMode = false;
			}
		}

		void SplitLineToTokens(string file, int i, string line, List<Token> result) {
			var separators = new char[] { ' ', '\t', '\r' };
			int from = 0;
			for (int j = 0; j < line.Length; j++) {
				char c = line[j];
				if (separators.Contains(c)) {
					if (from < j) {
						var newToken = new Token(
							line.Substring(from, j - from),
							new CodePosition(file, i + 1, from + 1)
						);
						ReadToken(result, newToken);
					}
					from = j + 1;
				}
			}
			if (from < line.Length) {
				var newToken = new Token(line.Substring(from, line.Length - from), new CodePosition(file, i + 1, from + 1));
				ReadToken(result, newToken);
			}
			ignoreToEndOfLine = false;
		}

		List<Token> SplitToTokens(string file, string code) {
			List<Token> result = new List<Token>();
			var lines = code.Split('\n');
			for (int i = 0; i < lines.Length; i++) {
				string line = lines[i];
				SplitLineToTokens(file, i, line, result);
			}
			ignoreMode = false;
			return result;
		}

		private void Build(string file) {
			if (files.Contains(file)) {
				return;
			}
			files.Add(file);
			var code = File.ReadAllText(file);
			//Debug.LogFormat("Compiling program:\n{0}", program);
			var newTokens = SplitToTokens(file, code);
			for (int i = 0; i < newTokens.Count-1; i++) {
				if (newTokens[i] == USE) {
					var fileToken = newTokens[i + 1];
					var fileName = GetFileName(file, fileToken);
					if (fileName != null) {
						Build(fileName);
					} else {
						throw new CompileError(
							string.Format("File not found: {0}", fileToken.text),
							fileToken.position,
							trace
						);
					}
				}
			}
			tokens.AddRange(newTokens);
		}

		void CalculateCallsGraph() {
			functions.Values.ForEach(f => {
				var df = f as DefinedFunction;
				if (df != null) {
					df.calls = df.body.Calls();
					df.calls.ForEach(called => {
						called.isCalledBy.Add(df);
					});
				}
			});
		}

		void FindRecursiveFunctions() {
			var order = new List<DefinedFunction>();
		}

		void Optimize() {
			CalculateCallsGraph();
			FindRecursiveFunctions();
			functions.ToList().ForEach(f => f.Value.Optimize());
		}

		void Compile() {
			CreateBuiltinFunctions();
			FindFunctionDefinitions();
			ReadProgram();
			//Optimize();
			//Debug.LogFormat("Functions: {0}", functions.Values.ExtToString());
			//Debug.LogFormat("Assertions: {0}", asserts.Count);
			asserts.ForEach(a => {
				//Debug.LogFormat("asserting {0}", a);
				var w = new System.Diagnostics.Stopwatch();
				w.Start();
				var from = callCnt;
				maxDepth = 0;
				var assertResult = a.Evaluate().Calculate();
				if (assertResult == null) {
					Debug.LogFormat("ASSERTION FAILED {0}", a);
				}		
				if (Debug.verbosity >= 2) {
					Debug.LogFormat(
						"assert {0} [{1} ms, {2} calls, {3} depth]",
						a,
						w.ElapsedMilliseconds, 
						callCnt - from,
						maxDepth
					);
				}
			});
			explain.ForEach(a => {
				//Debug.LogFormat("asserting {0}", a);
				var assertResult = a.Evaluate(explain: true).Calculate(explain: true);
				if (assertResult == null) {
					Debug.LogFormat("ASSERTION FAILED {0}", a);
				}
			});
			if (Debug.verbosity >= 1) {
				Debug.LogFormat("Asserts: {0}", asserts.Count);
				outputs.ForEach(o => {
					Debug.LogFormat("output: {0} is {1}", o, Object.Structure(o.Evaluate().Calculate()));
				});
			}
			Debug.LogFormat(4, "Functions: {0}", functions.ExtToString(
				delimiter: "\n\n",
				format: "{0}",
				elementToString: f => {
					var df = f.Value as DefinedFunction;
					if (df != null) {
						return df.Source();
					}
					return f.Value.ToString();
				}
			));
		}

		public void Run() {
			if (!functions.ContainsKey(MAIN)) {
				Debug.LogFormat("no main function found");
				return;
			}
			currentFunction = () => functions[MAIN].Call() as CommandObject;
			while (currentFunction != null) {
				var command = currentFunction();
				currentFunction = null;
				if (command != null) {
					currentFunction = command.command();
				}
			}
			//Debug.LogFormat("{1} = {0}", Object.Structure(result), functions["main"].ToString());
		}

		public string FunctionPosition(Function f) {
			var df = f as DefinedFunction;
			if (df == null) return "&";
			var position = df.position;
			if (position == null) return "?";
			if (df.body == null) {
				return position + " ?";
			}
			return position.ToString();
		}

		public Program() {
		}

		public Program(string fileName) {
			try {
				Build(fileName);
				Compile();
			} catch (PairError e) {
				error = e;
				Debug.LogFormat(e.Message);
				if (e is CompileError) {
					Debug.Log();
					if (last != null) {
						Debug.LogFormat("Last defined function: {0} ({1})", last.Source(), last.position);
					} else {
						Debug.LogFormat("No defined functions");
					}
					//Debug.Log();
					//Debug.LogFormat(
					//	"Defined functions: {0}",
					//	functions.OrderBy(f => f.Key).Select(f => "{0}[{2}] ({1})".i(
					//		f.Key,
					//		FunctionPosition(f.Value),
					//		f.Value.arguments.Count
					//	)).ExtToString(delimiter: "\n", format: "\n{0}")
	                //);
					//Debug.Log();
					//Debug.LogFormat("tokens: {0}", tokens.ExtToString(
					//	delimiter: "\n", 
					//	format: "{0}", 
					//	elementToString: t => t.text
					//));
				}
			}
		}
	}
}