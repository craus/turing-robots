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

	public bool finished = false;

	public Figure robot;

	string MakeAction() {
		Command c = current.GetComponent<Command>();
		if (c == null) {
			return "no command attached: #{0}".i(current.id);
		}
		return c.Run(this);
	}

	public Node FindStartNode() {
		Node source = GraphEditor.instance.startNode;
		if (source != null) {
			return source;
		}
		source = Node.nodesByID.Values.FirstOrDefault(n => !Node.nodesByID.Values.Any(n1 => n1.left.to == n || n1.right.to == n));
		if (source != null) {
			return source;
		}
		return Node.nodesByID.Values.MinBy(n => n.id);
	}

	public void Step() {
		if (finished) {
			current = null;
			return;
		}

		if (current == null) {
			current = FindStartNode();
			if (current != null) {
				Debug.LogFormat("Start node found: {0}", current.id);
			}
			return;
		}
		string action = MakeAction();
		Debug.LogFormat("Action: {0}", action);

		if (current == null) {
			return; // MakeAction destroyed current vertex (in case of run end)
		}

		current = collided ? current.right.to : current.left.to;
		Debug.LogFormat("Proceeding to node #{0}", current.id);
	}

	public void Update() {
		pointer.gameObject.SetActive(current != null);
		if (current != null) {
			pointer.transform.SetParent(current.transform, worldPositionStays: false);
		}
	}

	public void Start() {
		CommandMachine.instance.interpreters.Add(this);
	}

	public void OnDestroy() {
		if (pointer != null) {
			Destroy(pointer.gameObject);
		}
		if (CommandMachine.instance != null) {
			CommandMachine.instance.interpreters.Remove(this);
		}
	}
}
