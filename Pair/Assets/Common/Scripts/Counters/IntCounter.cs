using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IntCounter : Counter<int> {
	public override  string StringValue(){
		return value.ToString();
	}
}
