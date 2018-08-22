using System.Collections.Generic;

public class Pair : BuiltinFunction
{
	public Pair() {
		name = "pair";
		arguments.Add(new Argument("x", 0));
		arguments.Add(new Argument("y", 1));
	}

	protected override PairObject CallInternal(params Calculatable[] arguments) {
		return new PairObject(arguments[0].Calculate(), arguments[1].Calculate());
	}
}