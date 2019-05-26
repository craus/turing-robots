using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IntCounterValueProvider : IntValueProvider {
	public IntCounter counter;
	public override int Value {
		get {
			return counter.value;
		}
	}
}
