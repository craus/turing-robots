using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class Player : MonoBehaviour {
	public static Player instance;

	public int details = 10;
	public int level = 0;

	public TMPro.TextMeshProUGUI text;

	public void Awake() {
		instance = this;
	}

	public void Win() {
		if (Node.nodesByID.Values.Count > details) {
			Reset();
		} else {
			NextLevel();
		}
	}

	public void Reset() {
		CommandMachine.instance.Reset();
		Board.instance.Reset();
	}

	public void NextLevel() {
		level += 1;

		details -= Node.nodesByID.Values.Count;
		details += 3;

		FindObjectOfType<Board>().Generate();
		GraphEditor.instance.Clear();
	}

	public void Update() {
		if (Input.GetButtonDown("Reset")) {
			Reset();
		}
		text.text = "Level: {0}\nDetails: {1}".i(level, details);
	}
}
