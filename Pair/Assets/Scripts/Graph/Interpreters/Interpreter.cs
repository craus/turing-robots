using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

[ExecuteInEditMode]
public class Interpreter : MonoBehaviour {
	public Node com;
	public Node head;
	public Node mark;

	[Space]
	public Node goRed;
	public Node goBlue;
	public Node swap;
	public Node linkRed;
	public Node linkBlue;

	[Space]
	public Node makeNew;
	public Node del;

	public Node CommandHead { get {	return com.left.to;	} }
	public Node WorkHead { get { return head.left.to; }	}
	public Node MarkHead { get { return mark.left.to; }	}

	public Node GoRedAction { get {	return goRed.left.to; } }
	public Node GoBlueAction { get { return goBlue.left.to;	} }
	public Node SwapAction { get { return swap.left.to; } }
	public Node LinkRedAction {	get { return linkRed.left.to; }	}
	public Node LinkBlueAction { get { return linkBlue.left.to; } }

	public Node CreateNodeAction { get { return makeNew.left.to; } }
	public Node DeleteNodeAction { get { return del.left.to; } }

	public Node CurrentCommand { get { return CommandHead.right.to; } }
	public Node NextCommand { get { return CurrentCommand.left.to; } }
	public Node CurrentAction { get { return CurrentCommand.right.to; } }

	string MakeAction() {
		if (CurrentAction == GoRedAction) {
			WorkHead.right.to = WorkHead.right.to.left.to;	
			return "head moved red to #{0}".i(WorkHead.right.to.id);
		} else if (CurrentAction == GoBlueAction) {
			WorkHead.right.to = WorkHead.right.to.right.to;
			return "head moved blue to #{0}".i(WorkHead.right.to.id);
		} else if (CurrentAction == SwapAction) {
			var temp = WorkHead.right.to;
			WorkHead.right.to = MarkHead.right.to;
			MarkHead.right.to = temp;
			return "head and mark swapped";
		} else if (CurrentAction == LinkRedAction) {
			MarkHead.right.to.left.to = WorkHead.right.to;
			return "red link from #{0} directed to #{1}".i(MarkHead.right.to, WorkHead.right.to);
		} else if (CurrentAction == LinkBlueAction) {
			MarkHead.right.to.right.to = WorkHead.right.to;
			return "blue link from #{0} directed to #{1}".i(MarkHead.right.to, WorkHead.right.to);
		} else if (CurrentAction == CreateNodeAction) {
			var newNode = Instantiate(SelfModifyingGraphEditor.instance.nodeSample);
			newNode.transform.position = UnityEngine.Random.insideUnitCircle * 10;
			WorkHead.right.to = newNode;
			return "head moved to newly created #{0}".i(WorkHead.right.to.id);
		} else if (CurrentAction == DeleteNodeAction) {
			var toDelete = WorkHead.right.to;
			GraphEditor.instance.Delete(toDelete);
			WorkHead.right.to = MarkHead.right.to;
			return "deleted #{0}, head moved to #{1}".i(toDelete.id, WorkHead.right.to.id);
		} else {
			return "unknown command, nothing happened";
		}
	}

	public void Step() {
		string action = MakeAction();
		Debug.LogFormat("Action: {0}", action);

		CommandHead.right.to = NextCommand;
	}

	public void Update() {
		if (Input.GetButtonDown("Step")) {
			Step();
		}
	}
}
