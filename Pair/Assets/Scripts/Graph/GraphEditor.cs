using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class GraphEditor : MonoBehaviour {
	const string graphFile = "graph.dat";

	public static GraphEditor instance;

	public Node nodeSample;
	public Link linkSample;

	public Node hovered;
	public Vector2 dragRelativePosition;
	public Vector2 cameraDragRelativePosition;

	public Link left;
	public Link right;

	public bool dragged = false;
	public bool panned  = false;

	public void Awake() {
		instance = this;
	}

	public Vector2 Mouse {
		get {
			return Camera.main.ScreenToWorldPoint(Input.mousePosition);
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

	void Zoom(float times) {
		var oldMouse = Mouse;
		Camera.main.orthographicSize /= times;
		Camera.main.transform.localScale /= times;
		Camera.main.transform.position += (oldMouse - Mouse).WithZ(0);
	}

	void Zoom() {
		Zoom(Mathf.Pow(2f, Input.GetAxis("Mouse ScrollWheel")));
		if (Input.GetAxis("Mouse ScrollWheel") != 0) {
			UpdateAllLinks();
		}
	}

	void Drag() {
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

	void UpdateAllLinks() {
		Link.links.ForEach(l => l.Update());
	}

	void Pan() {
		if (Input.GetButtonDown("Pan") && hovered == null && left == null && right == null) {
			cameraDragRelativePosition = Mouse;
			panned = false;
		}
		if (Input.GetButton("Pan") && hovered == null && left == null && right == null) {
			var delta = (cameraDragRelativePosition - Mouse).WithZ(0);
			Camera.main.transform.position += delta;
			if (delta != Vector3.zero) {
				panned = true;
				UpdateAllLinks();
			}
		}
		if (Input.GetButtonUp("Pan")) {
			panned = false;
		}
	}

	void CreateNode() {
		if (Input.GetButtonUp("Create Node") && hovered == null && !panned) {
			var newNode = Instantiate(nodeSample);
			newNode.transform.position = Mouse;
		}
	}

	void Delete(Link link) {
		Destroy(link.gameObject);
	}

	void Delete(Node node) {
		Link.links.ForEach(l => {
			if (l.from == node) {
				Delete(l);
			} else if (l.to == node) {
				l.to = l.from;
			}
		});
		Destroy(node.gameObject);
	}

	void DeleteNode() {
		if (Input.GetButtonUp("Delete Node") && mutableHovered != null && !dragged) {
			Delete(mutableHovered);
		}
	}

	void Connect() {
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

	void QuickSave() {
		if (Input.GetButtonDown("Quick Save")) {
			Save();
		}
	}

	void QuickLoad() {
		if (Input.GetButtonDown("Quick Load")) {
			Load();
		}
	}

	public void Update() {
		CreateNode();
		DeleteNode();
		Drag();
		Pan();
		Hover();
		Zoom();
		Connect();
		QuickSave();
		QuickLoad();
	}

	public GraphModel BuildGraphModel() {
		var result = new GraphModel();
		result.camera.position = Camera.main.transform.position;
		result.camera.zoom = Camera.main.orthographicSize;

		FindObjectsOfType<Node>().ForEach(node => {
			result.nodes.Add(new NodeModel(node.id, node.transform.position, node.left.to.id, node.right.to.id));
		});

		return result;
	}

	public void RestoreGraphModel(GraphModel graph) {
		Zoom(Camera.main.orthographicSize / graph.camera.zoom);
		Camera.main.transform.position = graph.camera.position.WithZ(Camera.main.transform.position.z);

		Node.nodesByID.Values.ForEach(node => {
			node.model = null;
		});

		graph.nodes.ForEach(nodeModel => {
			Node node;
			if (Node.nodesByID.ContainsKey(nodeModel.id)) {
				node = Node.nodesByID[nodeModel.id];
			} else {
				node = Instantiate(nodeSample);
				node.SetID(nodeModel.id);
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
