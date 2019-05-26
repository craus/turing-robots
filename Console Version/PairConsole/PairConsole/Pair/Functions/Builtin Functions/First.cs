using System.Collections.Generic;

public class First : BuiltinFunction
{
	public First() {
		name = "first";
		arguments.Add(new Argument("p", 0));
	}

	protected override PairObject CallInternal(params Calculatable[] arguments) {
		var pair = arguments[0].Calculate();
		if (pair == null) {
			return null;
		}
		return pair.first;
	}
}