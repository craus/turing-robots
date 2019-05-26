using UnityEngine;

public class PairObject
{
	public static int lastID;

	public int id;

	public PairObject first;
	public PairObject second;

	public PairObject(PairObject first, PairObject second) {
		this.first = first;
		this.second = second;
		++lastID;
		this.id = lastID;
		Debug.LogFormat("{0} = pair {1} {2}", ToString(this), ToString(first), ToString(second));
	}

	public override string ToString() {
		return "#" + id.ToString();
	}

	public string Structure() {
		return string.Format("pair({0}, {1})", Structure(first), Structure(second));
	}

	public static string Structure(PairObject p) {
		return p != null ? p.Structure() : "nil";
	}

	public static string ToString(PairObject p) {
		return p != null ? p.ToString() : "#0";
	}
}