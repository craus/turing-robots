using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class Lose : MonoBehaviour {
	public void Activate() {
		Destroy(Board.instance.robot.Get().gameObject);
		CommandGraphInterpreter.instance.finished = true;
	}
}
