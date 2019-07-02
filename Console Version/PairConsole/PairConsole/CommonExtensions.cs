using System;
using System.Collections.Generic;
using System.Linq;

public static class CommonExtensions
{
	public static string ExtToString<T>(this IEnumerable<T> collection, string delimiter = ", ", string format = "[{0}]", Func<T, string> elementToString = null) {
		elementToString = elementToString ?? (obj => obj.ToString());
		return String.Format(format, String.Join(delimiter, collection.Select(obj => obj != null ? elementToString(obj) : "null").ToArray()));
	}

	public static string i(this string s, params object[] args) {
		return string.Format(s, args);
	}

	public static IEnumerable<T> Yield<T>(this T item) {
		yield return item;
	}

	public static IEnumerable<T> Empty<T, T1>(this T1 anyObject) {
		yield break;
	}

	public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
		foreach (T element in source) {
			action(element);
		}
	}
}

