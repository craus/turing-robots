using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class Board : MonoBehaviour {
	public Cell cellPrefab;
	public Figure wallPrefab;
	public Figure exitPrefab;
	public List<Figure> startPrefabs;
	public List<CommandGraphInterpreter> interpreters;
	public Figure minePrefab;

	public List<Figure> robots;
	public List<Figure> starts;

	public bool completed = false;

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

	public void Clear() {
		if (Extensions.Editor()) {
			cellParent.transform.Children().ForEach(c => DestroyImmediate(c.gameObject));
		} else {
			cellParent.transform.Children().ForEach(c => Destroy(c.gameObject));
		}
	}

	public void CreateBlank(int n, int m) {
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
			}
		}
	}

	[ContextMenu("Generate")]
	public void Generate() {
		Clear();
		CreateBlank(n,m);

		for (int i = 0; i < n; i++) {
			for (int j = 0; j < m; j++) {
				if (Rand.rndEvent(0.12f)) {
					Instantiate(wallPrefab).Place(cells[i,j]);
				} else if (Rand.rndEvent(0.03f)) {
					Instantiate(minePrefab).Place(cells[i,j]);
				}
			}
		}

		int robotsCount = 1;

		starts.Clear();

		for (int i = 0; i < robotsCount; i++) {
			starts.Add(Instantiate(startPrefabs[i]).Place(Rand.rnd(cells, c => c.figures.Count == 0)));
		}
		Reset();

		Instantiate(exitPrefab).Place(Rand.rnd(cells, c => c.figures.Count == 0));
	}

	public void Reset() {
		completed = false;
		robots.ForEach(r => Extensions.Destroy(r.gameObject));
		robots.Clear();
		interpreters.Clear();
		starts.ForEach(s => {
			var robot = s.GetComponent<RobotStart>().Spawn();
			robots.Add(robot);
			var interpreter = robot.GetComponentInChildren<CommandGraphInterpreter>();
			interpreter.Update();
			interpreters.Add(interpreter);
		});
	}
}
