//using System.Collections.Generic;
//using System.Linq;

//namespace Pair
//{
//	public class Lambda : Expression
//	{
//		public List<Argument> arguments = new List<Argument>();

//		public override Calculatable Evaluate(params Calculatable[] argumentValues) {
//			return new Calculatable(function, arguments.Select(expr => expr.Evaluate(argumentValues)).ToList());
//		}

//		public override string ToString() {
//			return "{0}{1}".i(function.name, arguments.Count > 0 ? arguments.ExtToString(format: "({0})") : "");
//		}
//	}
//}