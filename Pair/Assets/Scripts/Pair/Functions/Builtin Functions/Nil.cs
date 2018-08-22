using System.Collections.Generic;

public class Nil : BuiltinFunction
{
	public Nil() {
		name = "nil";
	}

	protected override PairObject CallInternal(params Calculatable[] arguments) {
		return null;
	}
}