using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoolCounterValueProvider : BoolValueProvider {
	public BoolCounter counter;
	public override bool Value {
		get {
			return counter.value;
		}
	}
}
