using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEngine;
#endif
using System.Linq;
using System;

public class PairProgram
{
	const string IS = "is";
	const string ASSERT = "assert";
	const string OUTPUT = "output";

	public Dictionary<string, Function> functions = new Dictionary<string, Function>();
	public List<Expression> asserts = new List<Expression>();
	public List<Expression> outputs = new List<Expression>();

	public string[] tokens;
	public int cur;

	private Expression ReadExpression(Function definingFunction = null) {
		if (definingFunction != null) {
			var arg = definingFunction.arguments.FirstOrDefault(a => a.name == tokens[cur]);
			if (arg != null) {
				++cur;
				return arg;
			}
		}
		var exp = new FunctionCall();
		if (!functions.ContainsKey(tokens[cur])) {
			Debug.LogFormat("Functions: {0}", functions.ExtToString());
			throw new Exception("No such function: '{0}'".i(tokens[cur]));
		}
		exp.function = functions[tokens[cur]];
		//Debug.LogFormat("Reading function call: {0}", exp.function.name);
		++cur;
		for (int i = 0; i < exp.function.arguments.Count; i++) {
			exp.arguments.Add(ReadExpression(definingFunction));
		}
		return exp;
	}

	void FindFunctionDefinitions() {
		Map<string, int> shortestIs = new Map<string, int>();
		for (int i = 0; i < tokens.Length; i++) {
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
			for (int i = 0; i < shortestIs[key]; i++) {
				functions[key].arguments.Add(new Argument("", i));
			}
		});
	}

	void ReadFunctionDefinition() {
		var f = functions[tokens[cur]] as DefinedFunction;
		if (f.body != null) {
			Debug.LogFormat("Functions: {0}", functions.ExtToString());
			throw new Exception("Function already exists: '{0}'".i(f.name));
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
	}

	void ReadAssert() {
		++cur;// skip "assert"	
		asserts.Add(ReadExpression());
	}

	void ReadOutput() {
		++cur;// skip "output"	
		outputs.Add(ReadExpression());
	}

	private void ReadProgram() {
		cur = 0;
		while (cur < tokens.Length) {
			if (tokens[cur] == ASSERT) {
				ReadAssert();
			} else if (tokens[cur] == OUTPUT) {
				ReadOutput();
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

	public void Compile(string program) {
		//Debug.LogFormat("Compiling program:\n{0}", program);
		tokens = program.Split(new char[] { ' ', '\t', '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
		CreateBuiltinFunctions();
		FindFunctionDefinitions();
		ReadProgram();
		//Debug.LogFormat("Functions: {0}", functions.Values.ExtToString());
		//Debug.LogFormat("Assertions: {0}", asserts.Count);
		asserts.ForEach(a => {
			Debug.LogFormat("asserting {0}", a);
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

	public PairProgram(string code) {
		Compile(code);
	}
}