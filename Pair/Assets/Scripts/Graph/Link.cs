﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

[ExecuteInEditMode]
public class Link : MonoBehaviour {
	public float arrowLength = 0.1f;
	public float arrowWidth = 3f;
	public float arrowBarrierLength = 0.01f;

	public LineRenderer line;

	public static HashSet<Link> links = new HashSet<Link>();

	public Node from;
	public Node to;

	public bool useTemporalTo;
	public Vector2 temporalTo;
	public float temporalToRadius;

	public bool UseTemporalTo {
		get {
			if (Extensions.Editor()) {
				return false;
			}
			return GraphEditor.instance.left == this || GraphEditor.instance.right == this;
		}
	}
	public Vector2 TemporalTo {
		get {
			return GraphEditor.instance.mutableHovered == null 
				? GraphEditor.instance.Mouse 
					: GraphEditor.instance.mutableHovered.transform.position.xy();
		}
	}
	public float TemporalToRadius {
		get {
			return GraphEditor.instance.mutableHovered == null ? 0 : GraphEditor.instance.mutableHovered.Radius;
		}
	}

	public void Update() {
		if (to == null) {
			to = from;
		}
		if (!Extensions.Editor()) {
			useTemporalTo = UseTemporalTo;
			temporalTo = TemporalTo;
			temporalToRadius = TemporalToRadius;
		} 
		Vector2 a = from.transform.position;
		Vector2 f = UseTemporalTo ? TemporalTo : to.transform.position.xy();
		Vector2 dir = (f-a).normalized;
		float r1 = from.Radius;
		float r2 = UseTemporalTo ? TemporalToRadius : to.Radius;
		Vector2 b = a + dir * r1 * 0.99f;
		Vector2 e = f - dir * r2;
		Vector2 d = e - dir * arrowLength;
		Vector2 c = d - dir * arrowBarrierLength;
		float len = Vector2.Distance(b, e);
		if (len > arrowLength + arrowBarrierLength) {
			line.positionCount = 4;
			line.SetPositions(new Vector3[] { b, c, d, e });
			line.widthCurve = new AnimationCurve(
				new Keyframe(0.0f, 0.4f),
				new Keyframe(0.999f - arrowLength / len, 0.4f),
				new Keyframe(1 - arrowLength / len, 1f),
				new Keyframe(1, 0f)
			);
		} else {
			line.positionCount = 2;
			line.SetPositions(new Vector3[] { b, e });
			line.widthCurve = new AnimationCurve(
				new Keyframe(0.0f, 1f),
				new Keyframe(1, 0f)
			);
		}
	}

	public void Start() {
		if (!Extensions.Editor()) {
			links.Add(this);
			Update();
		}
	}

	void OnDestroy()
	{
		links.Remove(this);
	}
}
