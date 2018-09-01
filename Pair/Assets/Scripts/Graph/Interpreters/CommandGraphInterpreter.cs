using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class CommandGraphInterpreter : MonoBehaviour {
	public Transform pointer;

	public Node current = null;

	public bool collided = false;

	public bool playing = false;

	string MakeAction() {
		Command c = current.GetComponent<Command>();
		if (c == null) {
			return "no command attached: #{0}".i(current.id);
		}
		return c.Run();
	}

	public Node FindStartNode() {
		var source = Node.nodesByID.Values.FirstOrDefault(n => !Node.nodesByID.Values.Any(n1 => n1.left.to == n || n1.right.to == n));
		if (source != null) {
			return source;
		}
		return Node.nodesByID.Values.MinBy(n => n.id);
	}

	public void Step() {
		if (current == null) {
			current = FindStartNode();
			Debug.LogFormat("Start node found: {0}", current.id);
			return;
		}
		string action = MakeAction();
		Debug.LogFormat("Action: {0}", action);

		current = collided ? current.right.to : current.left.to;
		Debug.LogFormat("Proceeding to node #{0}", current.id);
	}

	public void Update() {
		if (Input.GetButtonDown("Step")) {
			Step();
		}
		if (Input.GetButtonDown("Pause")) {
			playing = false;
		}
		if (Input.GetButtonDown("Play")) {
			playing = true;
		}
		pointer.gameObject.SetActive(current != null);
		if (current != null) {
			pointer.position = current.transform.position.Change(z: pointer.position.z);
		}

	}
}
