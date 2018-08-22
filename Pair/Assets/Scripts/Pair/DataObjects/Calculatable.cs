using System.Collections.Generic;
using UnityEngine;

public class Calculatable
{
	public Function function;
	public List<Calculatable> arguments;

	public Calculatable(Function function, List<Calculatable> arguments) {
		this.function = function;
		this.arguments = arguments;
	}

	public PairObject Calculate() {
		//Debug.LogFormat("{0} = ?", this);
		var result = function.Call(arguments.ToArray());
		//Debug.LogFormat("{0} = {1}", this, PairObject.ToString(result));
		return result;
	}

	public override string ToString() {
		return string.Format("{0}{1}", function.name, arguments.Count > 0 ? "({0})".i(arguments.ExtToString(format: "{0}")) : "");
	}
}