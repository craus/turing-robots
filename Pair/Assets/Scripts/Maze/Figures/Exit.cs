using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class Exit : MonoBehaviour {
	public void Activate() {
		FindObjectOfType<Board>().Generate();
		GraphEditor.instance.Clear();
	}
}
