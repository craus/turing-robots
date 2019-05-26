using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class Player : MonoBehaviour {
	public static Player instance;

	public int details = 20;
	public int level = 1;

	public int detailsPerLevel = 5;

	public bool won;

	public TMPro.TextMeshProUGUI text;

	public void Awake() {
		instance = this;
	}

	public void Win() {
		CommandMachine.instance.finished = true;
		if (Node.nodesByID.Values.Count > details) {
		} else {
			won = true;
		}
	}

	public void Reset() {
		CommandMachine.instance.Reset();
		MazeProblemManager.instance.Reset();
	}

	public void NextLevel() {
		won = false;
		level += 1;

		details -= Node.nodesByID.Values.Count;
		details += detailsPerLevel;

		MazeProblemManager.instance.Generate();
		GraphEditor.instance.Clear();
	}

	public string Status() {
		if (CommandMachine.instance.launched) {
			if (CommandMachine.instance.finished) {
				return "{1} #{0}".i(CommandMachine.instance.steps, won ? "Win" : "Dead");
			} else {
				return "Playing #{0}".i(CommandMachine.instance.steps);
			}
		} else {
			return "Editing";
		}
	}

	public void Update() {
		if (Input.GetButtonDown("Reset")) {
			Reset();
		}
		if (Input.GetButtonDown("Next Level")) {
			if (won) {
				NextLevel();
			}
		}
		text.text = "Level: {0}\nDetails: {1}\n{2}".i(level, details, Status());
	}
}
