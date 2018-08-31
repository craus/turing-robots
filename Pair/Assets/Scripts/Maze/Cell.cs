using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class Cell : MonoBehaviour {
	public int x;
	public int y;
	public Board board;

	public List<Figure> figures;

	public Cell Shifted(int dx, int dy) {
		return board.GetCell(x + dx, y + dy);
	}
}
