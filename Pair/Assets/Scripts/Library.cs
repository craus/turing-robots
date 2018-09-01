using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class Library : MonoBehaviour {
	public static Library instance;

	public void Awake() {
		instance = this;
	}

	public GameObject wallType;
	public GameObject robotType;
	public GameObject exitType;
}
