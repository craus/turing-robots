using System.Collections.Generic;
using System.Linq;

public class FunctionCall : Expression
{
	public Function function;
	public List<Expression> arguments = new List<Expression>();

	public override Calculatable Evaluate(params Calculatable[] argumentValues) {
		return new Calculatable(function, arguments.Select(expr => expr.Evaluate(argumentValues)).ToList());
	}

	public override string ToString() {
		return "{0}{1}".i(function.name, arguments.ExtToString(format: "({0})"));
	}
}