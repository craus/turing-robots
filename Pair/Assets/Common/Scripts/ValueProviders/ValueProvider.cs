using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class ValueProvider<T> : AbstractValueProvider {
	public abstract T Value {
		get;
	}

	public T value;

	public void Update() {
		value = Value;
	}
}
