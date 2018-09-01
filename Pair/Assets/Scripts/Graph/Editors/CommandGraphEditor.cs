using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class CommandGraphEditor : GraphEditor {
	public new static CommandGraphEditor instance;

	[Space]

	public Node upCommand;
	public Node downCommand;
	public Node leftCommand;
	public Node rightCommand;
	public Node wallCommand;

	public List<Node> commandNodes;

	public void Start() {
		commandNodes.ForEach(cn => cn.type = commandNodes.IndexOf(cn));
	}

	public override bool SatisfyModel(Node node, NodeModel model) {
		return node.type == model.type;
	}

	public override Node CreateNodeForModel(NodeModel model) {
		var node = Instantiate(commandNodes[model.type]);
		node.SetID(model.id);
		return node;
	}

	protected override bool Frozen() {
		return CommandGraphInterpreter.instance.launched;
	}

	protected override void CreateNode() {
		if (hovered != null || panned) {
			return;
		}
		if (Input.GetButtonDown("Up")) {
			CreateNode(upCommand);
		}
		if (Input.GetButtonDown("Down")) {
			CreateNode(downCommand);
		}
		if (Input.GetButtonDown("Left")) {
			CreateNode(leftCommand);
		}
		if (Input.GetButtonDown("Right")) {
			CreateNode(rightCommand);
		}
		if (Input.GetButtonDown("Wall")) {
			CreateNode(wallCommand);
		}
	}
}
