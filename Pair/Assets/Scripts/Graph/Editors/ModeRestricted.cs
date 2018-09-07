using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEngine.PostProcessing;

public class ModeRestricted : MonoBehaviour {
	public GameObject target;
	public UI.Mode mode;

	public void Update() {
		target.SetActive(mode == UI.instance.mode);
	}
}
