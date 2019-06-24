using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEngine;
#endif
using System.Linq;
using System;
using System.IO;

public class PairProgram
{
	const string IS = "is";
	const string ASSERT = "assert";
	const string OUTPUT = "output";
	const string USE = "use";

	public Dictionary<string, Function> functions = new Dictionary<string, Function>();
	public List<Expression> asserts = new List<Expression>();
	public List<Expression> outputs = new List<Expression>();

	public List<Token> tokens = new List<Token>();
	public int cur;

	public HashSet<string> files = new HashSet<string>();

	public CompileError error;

	public Stack<string> trace = new Stack<string>();

	private Expression ReadExpression(Function definingFunction = null) {
		trace.Push("read expression {0}{1}".i(
			tokens[cur],
			functions.ContainsKey(tokens[cur])
				? " ({0} args)".i(functions[tokens[cur]].arguments.Count)
				: ""
		));
		if (definingFunction != null) {
			var arg = definingFunction.arguments.FirstOrDefault(a => a.name == tokens[cur]);
			if (arg != null) {
				++cur;
				trace.Pop();
				return arg;
			}
		}
		var exp = new FunctionCall();
		if (!functions.ContainsKey(tokens[cur])) {
			//Debug.LogFormat("Functions: {0}", functions.ExtToString());
			throw new CompileError("No such function: '{0}'".i(tokens[cur].text), tokens[cur].position, trace);
		}
		exp.function = functions[tokens[cur]];
		//Debug.LogFormat("Reading function call: {0}", exp.function.name);
		++cur;
		for (int i = 0; i < exp.function.arguments.Count; i++) {
			exp.arguments.Add(ReadExpression(definingFunction));
		}
		trace.Pop();
		return exp;
	}

	void FindFunctionDefinitions() {
		Map<string, int> shortestIs = new Map<string, int>(() => int.MaxValue);
		for (int i = 0; i < tokens.Count; i++) {
			if (tokens[i] == IS) {
				for (int j = 1; j <= i; j++) {
					if (tokens[i - j] == IS) {
						break;
					}
					shortestIs[tokens[i - j]] = Math.Min(shortestIs[tokens[i - j]], j);
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
			for (int i = 0; i < shortestIs[key]-1; i++) {
				functions[key].arguments.Add(new Argument("", i));
			}
		});
	}

	void ReadFunctionDefinition() {
		trace.Push("read function definition {0}".i(tokens[cur]));
		var f = functions[tokens[cur]] as DefinedFunction;
		if (f == null) {
			throw new CompileError("Trying to define function: '{0}'".i(tokens[cur].text), tokens[cur].position, trace);
		}
		if (f.body != null) {
			//Debug.LogFormat("Functions: {0}", functions.ExtToString());
			throw new CompileError("Function already exists: '{0}'".i(f.name), tokens[cur].position, trace);
		}
		f.name = tokens[cur];
		f.arguments = new List<Argument>();
		//Debug.LogFormat("Defining function: {0}", f.name);
		++cur;
		while (tokens[cur] != IS) {
			f.arguments.Add(new Argument(tokens[cur], f.arguments.Count()));
			++cur;
		}
		++cur;// skip "is"	
		f.body = ReadExpression(f);
		//Debug.LogFormat("{0} body end", f.name);
		trace.Pop();
	}

	void ReadAssert() {
		++cur;// skip "assert"	
		asserts.Add(ReadExpression());
	}

	void ReadOutput() {
		++cur;// skip "output"	
		outputs.Add(ReadExpression());
	}

	void ReadUse() {
		cur += 2;// skip "use" and filename	
	}

	private void ReadProgram() {
		cur = 0;
		while (cur < tokens.Count) {
			if (tokens[cur] == ASSERT) {
				ReadAssert();
			} else if (tokens[cur] == OUTPUT) {
				ReadOutput();
			} else if (tokens[cur] == USE) {
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
					if (newToken.text[0] == '#') {
						return;
					}
					result.Add(newToken);
				}
				from = j + 1;
			}
		}
		if (from < line.Length) {
			result.Add(new Token(line.Substring(from, line.Length - from), new CodePosition(file, i + 1, from + 1)));
		}
	}

	List<Token> SplitToTokens(string file, string code) {
		List<Token> result = new List<Token>();
		var lines = code.Split('\n');
		for (int i = 0; i < lines.Length; i++) {
			string line = lines[i];
			SplitLineToTokens(file, i, line, result);
		}
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
		tokens.AddRange(newTokens);
		for (int i = 0; i < newTokens.Count; i++) {
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
	}

	void Compile() {
		CreateBuiltinFunctions();
		FindFunctionDefinitions();
		ReadProgram();
		//Debug.LogFormat("Functions: {0}", functions.Values.ExtToString());
		//Debug.LogFormat("Assertions: {0}", asserts.Count);
		asserts.ForEach(a => {
			//Debug.LogFormat("asserting {0}", a);
			var assertResult = a.Evaluate().Calculate();
			if (assertResult == null) {
				Debug.LogFormat("ASSERTION FAILED {0}", a);
			}
		});
		outputs.ForEach(o => {
			Debug.LogFormat("output: {0} is {1}", o, PairObject.Structure(o.Evaluate().Calculate()));
		});
	}

	public void Run() {
		if (!functions.ContainsKey("main")) {
			Debug.LogFormat("no main function found");
			return;
		}
		var result = functions["main"].Call();
		Debug.LogFormat("{1} = {0}", PairObject.Structure(result), functions["main"].ToString());
	}

	public PairProgram(string fileName) {
		try {
			Build(fileName);
			Compile();
		} catch (CompileError e) {
			error = e;
			Debug.LogFormat(e.Message);
		}
	}
}