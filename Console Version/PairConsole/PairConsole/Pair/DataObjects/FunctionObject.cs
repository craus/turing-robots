namespace Pair
{

	public class FunctionObject : Object
	{
		public Function f;

		public FunctionObject(Function f) {
			this.f = f;
		}

		public override string Structure() {
			return string.Format("function({0})", f);
		}
	}
}