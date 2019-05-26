using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoolCounter : Counter<bool> {
	public override string StringValue(){
		return value.ToString();
	}
}
