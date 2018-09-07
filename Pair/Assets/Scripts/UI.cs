using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEngine.PostProcessing;

public class UI : MonoBehaviour {
	public static UI instance;

	public PostProcessingBehaviour mazeSplitSelection;
	public PostProcessingBehaviour graphSplitSelection;

	public Camera mazeCamera;
	public Camera graphCamera;

	public enum Mode {
		None = 0, 
		Maze = 100,
		Graph = 200
	}

	public Mode mode;

	public void Awake() {
		instance = this;
	}

	public bool MouseOver(Camera c) {
		return new Rect(0, 0, 1, 1).Contains(c.ScreenToViewportPoint(Input.mousePosition));
	}

	public bool ModeLocked {
		get {
			return Input.GetButton("Pan") ||
				Input.GetButton("Drag") ||
				Input.GetButton("Link Left") ||
				Input.GetButton("Link Right");
		}
	}

	public void Update() {
		if (Input.GetKeyDown(KeyCode.Tab)) {
			mode = (mode == Mode.Maze ? Mode.Graph : Mode.Maze);
		}

		var viewportPoint = mazeCamera.ScreenToViewportPoint(Input.mousePosition);
		Debug.LogFormat("viewportPoint = {0}", viewportPoint);

		if (!ModeLocked) {
			if (MouseOver(mazeCamera)) {
				mode = Mode.Maze;
			} else if (MouseOver(graphCamera)) {
				mode = Mode.Graph;
			}
		}

		mazeSplitSelection.enabled = (mode == Mode.Maze);
		graphSplitSelection.enabled = (mode == Mode.Graph);
	}
}
