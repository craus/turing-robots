using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoolFormulaValueProvider : BoolValueProvider {
	public List<BoolValueProvider> arguments;
	public string formula;

	public bool Calculate(string formula) {
		if (formula.Contains('|')) {
			return formula.Split('|').Any(part => Calculate(part));
		}
		if (formula.Contains('&')) {
			return formula.Split('&').All(part => Calculate(part));
		}
		if (formula.Contains('-')) {
			return !Calculate(formula.Substring(1));
		}
		return arguments[int.Parse(formula)].Value;
	}

	public override bool Value {
		get {
			return Calculate(formula);
		}
	}
}
