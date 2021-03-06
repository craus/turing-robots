﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public abstract class GraphEditor : MonoBehaviour {
	const string graphFile = "graph.dat";

	public Node nodeSample;

	[Space]

	public static GraphEditor instance;

	public Node hovered;
	public Vector2 dragRelativePosition;
	public Vector2 cameraDragRelativePosition;

	public Camera graphCamera;

	public Link left;
	public Link right;

	public bool dragged = false;
	public bool panned  = false;

	public Node startNode;

	public void Awake() {
		instance = this;
	}

	public Vector2 Mouse {
		get {
			return graphCamera.ScreenToWorldPoint(Input.mousePosition);
		}
	}

	public Node mutableHovered {
		get {
			return hovered != null && !hovered.immutable ? hovered : null;
		}
	}

	public void Hover() {
		if (panned || dragged) {
			return;
		}

		hovered = null;

		var collider = Physics2D.OverlapPoint(Mouse);
		if (collider == null) {
			return;
		}

		hovered = collider.GetComponent<Node>();
	}

	protected void Zoom(float times) {
		var oldMouse = Mouse;
		graphCamera.orthographicSize /= times;
		graphCamera.transform.localScale /= times;
		graphCamera.transform.position += (oldMouse - Mouse).WithZ(0);
	}

	protected void Zoom() {
		Zoom(Mathf.Pow(2f, Input.GetAxis("Mouse ScrollWheel")));
		if (Input.GetAxis("Mouse ScrollWheel") != 0) {
			UpdateAllLinks();
		}
	}

	protected void Drag() {
		if (mutableHovered == null) {
			return;
		}
		if (Input.GetButtonDown("Drag") && left == null && right == null) {
			dragRelativePosition = hovered.transform.position.xy() - Mouse;
			dragged = false;
		}
		if (Input.GetButton("Drag") && left == null && right == null) {
			var newPosition = (Mouse + dragRelativePosition).WithZ(hovered.transform.position.z);
			if (newPosition != hovered.transform.position) {
				dragged = true;
			}
			hovered.transform.position = newPosition;
		}
		if (Input.GetButtonUp("Drag")) {
			dragged = false;
		}
	}

	protected void UpdateAllLinks() {
		Link.links.ForEach(l => l.Update());
	}

	protected void Pan() {
		if (Input.GetButtonDown("Pan") && hovered == null && left == null && right == null) {
			cameraDragRelativePosition = Mouse;
			panned = false;
		}
		if (Input.GetButton("Pan") && hovered == null && left == null && right == null) {
			var delta = (cameraDragRelativePosition - Mouse).WithZ(0);
			graphCamera.transform.position += delta;
			if (delta != Vector3.zero) {
				panned = true;
				UpdateAllLinks();
			}
		}
		if (Input.GetButtonUp("Pan")) {
			panned = false;
		}
	}

	protected virtual Node CreateNode(Node sample) {
		var newNode = Instantiate(sample);
		newNode.transform.position = Mouse;
		if (startNode == null) {
			startNode = newNode;
		}
		return newNode;
	}

	protected virtual void CreateNode() {
		if (Input.GetButtonUp("Create Node") && hovered == null && !panned) {
			CreateNode(nodeSample);
		}
	}

	protected void Delete(Link link) {
		Destroy(link.gameObject);
	}

	public void Delete(Node node) {
		Link.links.ForEach(l => {
			if (l.from == node) {
				Delete(l);
			} else if (l.to == node) {
				l.to = l.from;
			}
		});
		Destroy(node.gameObject);
	}

	protected void DeleteNode() {
		if (Input.GetButtonUp("Delete Node") && mutableHovered != null && !dragged) {
			Delete(mutableHovered);
		}
	}

	protected void Connect() {
		if (hovered != null) {
			if (Input.GetButtonDown("Link Left")) {
				Debug.LogFormat("Link Left");
				left = hovered.left;
			} 
			if (Input.GetButtonDown("Link Right")) {
				right = hovered.right;
			}
		}
		if (Input.GetButtonUp("Link Left")) {
			if (left != null) {
				left.to = mutableHovered;
			}
			left = null;
		} 
		if (Input.GetButtonUp("Link Right")) {
			if (right != null) {
				right.to = mutableHovered;
			}
			right = null;
		}
	}

	protected void QuickSave() {
		if (Input.GetButtonDown("Quick Save")) {
			Save();
		}
	}

	protected void QuickLoad() {
		if (Input.GetButtonDown("Quick Load")) {
			Load();
		}
	}

	public void Clear() {
		Node.nodesByID.Values.ForEach(n => Destroy(n.gameObject));
		Node.lastID = 0;
	}

	protected void CheckClear() {
		if (Input.GetButtonDown("Clear")) {
			Clear();
		}
	}

	protected virtual bool Frozen() {
		return false;
	}

	public void MarkStart() {
		if (hovered != null) {
			startNode = hovered;
		}
	}

	protected void CheckMarkStart() {
		if (Input.GetButtonDown("Make Start")) {
			MarkStart();
		}
	}

	public virtual void Update() {
		if (UI.instance.mode != UI.Mode.Graph) {
			return;
		}

		Pan();
		Zoom();
		Hover();
		QuickSave();

		if (!Frozen()) {
			CreateNode();
			DeleteNode();
			Drag();
			Connect();
			QuickLoad();
			CheckClear();
			CheckMarkStart();
		}
	}

	public virtual GraphModel BuildGraphModel() {
		var result = new GraphModel();
		result.camera.position = graphCamera.transform.position;
		result.camera.zoom = graphCamera.orthographicSize;

		FindObjectsOfType<Node>().ForEach(node => {
			result.nodes.Add(new NodeModel(node.id, node.type, node.transform.position, node.left.to.id, node.right.to.id));
		});

		result.lastNodeID = Node.lastID;
		result.startNodeID = startNode != null ? startNode.id : -1;

		return result;
	}

	public virtual bool SatisfyModel(Node node, NodeModel model) {
		return true;
	}

	public virtual Node CreateNodeForModel(NodeModel model) {
		var node = Instantiate(nodeSample);
		node.SetID(model.id);
		return node;
	}

	public virtual void RestoreGraphModel(GraphModel graph) {
		Zoom(graphCamera.orthographicSize / graph.camera.zoom);
		graphCamera.transform.position = graph.camera.position.WithZ(graphCamera.transform.position.z);

		Node.nodesByID.Values.ForEach(node => {
			node.model = null;
		});

		graph.nodes.ForEach(nodeModel => {
			Node node = null;
			if (Node.nodesByID.ContainsKey(nodeModel.id)) {
				node = Node.nodesByID[nodeModel.id];
				if (!SatisfyModel(node, nodeModel)) {
					Destroy(node.gameObject);
					node = null;
				}
			} 
			if (node == null) {
				node = CreateNodeForModel(nodeModel);
			}
			node.transform.position = nodeModel.position.WithZ(node.transform.position.z);
			node.model = nodeModel;
		});

		Node.nodesByID.Values.ForEach(n => {
			if (n.model == null) {
				Destroy(n.gameObject);
			} else {
				n.left.to = Node.nodesByID[n.model.left];
				n.right.to = Node.nodesByID[n.model.right];
			}
		});

		Node.lastID = graph.lastNodeID;

		startNode = Node.nodesByID[graph.startNodeID];
	}

	public void Save() {
		FileManager.SaveToFile(BuildGraphModel(), graphFile);
	}

	public void Load() {
		var savedModel = FileManager.LoadFromFile<GraphModel>(graphFile);
		if (savedModel != null) {
			RestoreGraphModel(savedModel);
		}
	}
}
