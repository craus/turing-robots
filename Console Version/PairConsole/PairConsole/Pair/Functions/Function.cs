using System.Collections.Generic;

public abstract class Function
{
	public string name;
	public List<Argument> arguments = new List<Argument>();

	//public Dictionary< hash;

	protected abstract PairObject CallInternal(params Calculatable[] arguments);

	public PairObject Call(params Calculatable[] arguments) {
		var result = CallInternal(arguments);
		return result;
	}
	public override string ToString() {
		return name;
		//return "{0}{1}".i(name, arguments.Count > 0 ? arguments.ExtToString(format: "({0})") : "");
	}
}