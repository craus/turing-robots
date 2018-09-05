using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class Lose : MonoBehaviour {
	public void Activate() {
		CommandMachine.instance.finished = true;
	}
}
