using System;
using UnityEngine;

[Serializable]
public class NodeModel
{
	public int id;
	public int type;

	public Vector2 position;
	
	public int left;
	public int right;

	public NodeModel() {
	}

	public NodeModel(int id, Vector2 position, int left, int right) {
		this.id = id;
		this.position = position;
		this.left = left;
		this.right = right;
	}

	public NodeModel(int id, int type, Vector2 position, int left, int right) {
		this.id = id;
		this.type = type;
		this.position = position;
		this.left = left;
		this.right = right;
	}
}