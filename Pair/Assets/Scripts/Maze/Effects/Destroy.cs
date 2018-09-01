using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class Destroy : MonoBehaviour {
	public void Activate(Figure f) {
		Destroy(f.gameObject);
	}
}
