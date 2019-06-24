

//public abstract class AbstractObject
//{
//	public static int lastID;

//	public int id;

//	public AbstractObject() {
//		++lastID;
//		this.id = lastID;
//		//Debug.LogFormat("{0} = pair {1} {2}", ToString(this), ToString(first), ToString(second));
//	}

//	public override string ToString() {
//		return "#" + id.ToString();
//	}

//	public abstract string Structure();

//	public static string Structure(PairObject p) {
//		return p != null ? p.Structure() : "nil";
//	}

//	public static string ToString(PairObject p) {
//		return p != null ? p.ToString() : "#0";
//	}
//}