using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEngine;
#endif
using System.Linq;
using System;

public class PairProgram
{
	const string IS = "is";

	public Dictionary<string, Function> functions = new Dictionary<string, Function>();

	public string[] tokens;
	public int cur;

	private Expression ReadExpression(Function definingFunction) {
		var arg = definingFunction.arguments.FirstOrDefault(a => a.name == tokens[cur]);
		if (arg != null) {
			++cur;
			return arg;
		}
		var exp = new FunctionCall();
		if (!functions.ContainsKey(tokens[cur])) {
			Debug.LogFormat("Functions: {0}", functions.ExtToString());
			throw new Exception("No such function: '{0}'".i(tokens[cur]));
		}
		exp.function = functions[tokens[cur]];
		Debug.LogFormat("Reading function call: {0}", exp.function.name);
		++cur;
		for (int i = 0; i < exp.function.arguments.Count; i++) {
			exp.arguments.Add(ReadExpression(definingFunction));
		}
		return exp;
	}

	private void ReadProgram() {
		cur = 0;
		while (cur < tokens.Length) {
			var f = new DefinedFunction();
			f.name = tokens[cur];	
			Debug.LogFormat("Defining function: {0}", f.name);
			++cur;
			while (tokens[cur] != IS) {
				f.arguments.Add(new Argument(tokens[cur], f.arguments.Count()));
				++cur;
			}
			++cur;// skip "is"	
			if (functions.ContainsKey(f.name)) {
				Debug.LogFormat("Functions: {0}", functions.ExtToString());
				throw new Exception("Function already exists: '{0}'".i(f.name));
			}
			functions.Add(f.name, f);
			f.body = ReadExpression(f);
			Debug.LogFormat("{0} body end", f.name);
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
		Debug.LogFormat("Compiling program:\n{0}", program);
		tokens = program.Split(new char[] { ' ', '\t', '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
		//Debug.LogFormat("Tokens: {0}", tokens.ExtToString());
		CreateBuiltinFunctions();
		//Debug.LogFormat("BuiltinFunctions: {0}", functions.ExtToString());
		ReadProgram();
		Debug.LogFormat("Functions: {0}", functions.Values.ExtToString());
	}

	public void Run() {
		var result = functions["main"].Call();
		Debug.LogFormat("{1} = {0}", PairObject.Structure(result), functions["main"].ToString());
	}
}