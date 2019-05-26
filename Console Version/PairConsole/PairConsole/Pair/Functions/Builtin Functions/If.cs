using System.Collections.Generic;

public class If : BuiltinFunction
{
	public If() {
		name = "if";
		arguments.Add(new Argument("cond", 0));
		arguments.Add(new Argument("when_true", 1));
		arguments.Add(new Argument("when_false", 2));
	}

	protected override PairObject CallInternal(params Calculatable[] arguments) {
		if (arguments[0].Calculate() != null) {
			return arguments[1].Calculate();
		} else {
			return arguments[2].Calculate();
		}
	}
}