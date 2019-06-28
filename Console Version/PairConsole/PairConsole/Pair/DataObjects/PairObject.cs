
namespace Pair
{
	public class PairObject : Object
	{
		public Calculatable first;
		public Calculatable second;

		public PairObject(Calculatable first, Calculatable second) {
			if (first == null || second == null) {
				throw new System.Exception("Pair with null");
			}
			this.first = first;
			this.second = second;
			++lastID;
			this.id = lastID;
			//Debug.LogFormat("{0} = pair {1} {2}", ToString(this), ToString(first), ToString(second));
		}

		public override string Structure() {
			return string.Format("pair({0}, {1})", Structure(first.Calculate()), Structure(second.Calculate()));
		}
	}
}