using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class Figure : MonoBehaviour {
	public Cell position;

	public Figure Place(Cell cell) {
		if (this.position != null) {
			this.position.figures.Remove(this);
		}
		this.position = cell;
		cell.figures.Add(this);
		this.transform.SetParent(cell.transform, worldPositionStays: false);
		this.transform.localPosition = Vector3.zero;
		return this;
	}

	public GameObject FigureType() {
		return GetComponent<FigureType>().type;
	}
}
