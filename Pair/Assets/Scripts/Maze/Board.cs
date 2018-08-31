using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class Board : MonoBehaviour {
	public Cell cellPrefab;
	public Figure wallPrefab;
	public Figure robotPrefab;
	public Figure exitPrefab;

	[Space]

	public Transform cellParent;
	float step = 0.55f;
	public Cell[,] cells;

	[Space]

	public int n;
	public int m;

	public bool Inside(int x, int y) {
		return 0 <= x && x < cells.GetLength(0) && 0 <= y && y < cells.GetLength(1);
	}

	public Cell GetCell(int x, int y) {
		if (!Inside(x, y)) {
			return null;
		}
		return cells[x, y];
	}

	[ContextMenu("Generate")]
	public void Generate() {
		if (Extensions.Editor()) {
			cellParent.transform.Children().ForEach(c => DestroyImmediate(c.gameObject));
		} else {
			cellParent.transform.Children().ForEach(c => Destroy(c.gameObject));
		}
		cells = new Cell[n, m];
		for (int i = 0; i < n; i++) {
			for (int j = 0; j < m; j++) {
				var cell = Instantiate(cellPrefab);
				cell.transform.SetParent(cellParent);
				cell.transform.position = new Vector3(
					(i - (n - 1) * 0.5f) * step,
					(j - (m - 1) * 0.5f) * step,
					0
				);
				cell.x = i;
				cell.y = j;
				cell.board = this;
				cells[i, j] = cell;

				if (Rand.rndEvent(0.3f)) {
					var wall = Instantiate(wallPrefab);
					wall.Place(cell);
				}
			}
		}

		var robot = Instantiate(robotPrefab).Place(Rand.rnd(cells, c => c.figures.Count == 0));
		GetComponent<ConstantFigure>().value = robot;

		Instantiate(exitPrefab).Place(Rand.rnd(cells, c => c.figures.Count == 0));
	}

	public void Start() {
		Generate();
	}
}
