using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class Viewer : MonoBehaviour {
	
	public Vector2 cameraDragRelativePosition;

	public bool panned = false;

	public new Camera camera;

	public Vector2 Mouse {
		get {
			return camera.ScreenToWorldPoint(Input.mousePosition);
		}
	}

	protected void Zoom(float times) {
		var oldMouse = Mouse;
		camera.orthographicSize /= times;
		camera.transform.localScale /= times;
		camera.transform.position += (oldMouse - Mouse).WithZ(0);
	}

	protected void Zoom() {
		Zoom(Mathf.Pow(2f, Input.GetAxis("Mouse ScrollWheel")));
	}

	protected void Pan() {
		if (Input.GetButtonDown("Pan")) {
			cameraDragRelativePosition = Mouse;
			panned = false;
		}
		if (Input.GetButton("Pan")) {
			var delta = (cameraDragRelativePosition - Mouse).WithZ(0);
			camera.transform.position += delta;
			if (delta != Vector3.zero) {
				panned = true;
			}
		}
		if (Input.GetButtonUp("Pan")) {
			panned = false;
		}
	}

	public void Update() {
		Pan();
		Zoom();
	}
}
