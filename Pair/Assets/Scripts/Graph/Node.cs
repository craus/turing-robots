﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class Node : MonoBehaviour {
	public Link left;
	public Link right;

	public SpriteRenderer circle;
	public SpriteRenderer startNodeMark;
	public TextMeshPro text;

	public Color hovered = Color.gray;
	public Color basic = Color.white;

	public bool immutable;

	public int id = -1;

	public int type = -1;

	public static int lastID = 0;

	public NodeModel model;

	public static Dictionary<int, Node> nodesByID = new Dictionary<int, Node>();

	public bool Hovered {
		get {
			return GraphEditor.instance.hovered == this;
		}
	}

	public float Radius {
		get {
			return transform.lossyScale.x;
		}
	}

	public void Update() {
		if (Extensions.Editor()) {
			if (immutable) {
				#if UNITY_EDITOR
				Undo.RecordObject(text, "text changed");
				#endif
				text.text = name;
				left.to = null;
				right.to = null;
			}
		} else {
			circle.color = Hovered ? hovered : basic;
			startNodeMark.enabled = this == GraphEditor.instance.startNode;
		}
	}

	public void SetID() {
		SetID(++lastID);
	}

	public void SetID(int id) {
		this.id = id;
		nodesByID[id] = this;
	}

	public void Start() {
		if (!Extensions.Editor()) {
			if (id == -1) {
				SetID();
			}
			if (!immutable) {
				name = "Node #{0}".i(id);
				left.name = "Left Link #{0}".i(id);
				right.name = "Right Link #{0}".i(id);
			}
		}
	}

	void OnDestroy()
	{
		nodesByID.Remove(id);
	}
}
