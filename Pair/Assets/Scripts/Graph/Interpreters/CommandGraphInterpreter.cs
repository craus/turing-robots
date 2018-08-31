using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

[ExecuteInEditMode]
public class CommandGraphInterpreter : MonoBehaviour {
	public Node current = null;

	public bool collided = false;

	string MakeAction() {
		Command c = current.GetComponent<Command>();
		if (c == null) {
			return "no command attached: #{0}".i(current.id);
		}
		return c.Run();
	}

	public Node FindStartNode() {
		return Node.nodesByID.Values.FirstOrDefault(n => !Node.nodesByID.Values.Any(n1 => n1.left.to == n || n1.right.to == n));
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
	}
}
