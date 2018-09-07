using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class MazeProblem : MonoBehaviour {
	public static MazeProblem instance;

	public TMPro.TextMeshProUGUI text;

	public Board boardPrefab;

	public Transform boardsRoot;

	public int n = 10;

	public List<Board> boards;

	public Board current;

	public void Awake() {
		instance = this;
	}

	[ContextMenu("Generate")]
	public void Generate() {
		boards.ForEach(b => {
			if (b != null) {
				Extensions.Destroy(b.gameObject);
			}
		});
		boards.Clear();
		for (int i = 0; i < n; i++) {
			var board = Instantiate(boardPrefab);
			board.gameObject.SetActive(false);
			boards.Add(board);
			board.transform.SetParent(boardsRoot, worldPositionStays: false);
			board.Generate();
		}
		SetCurrent(boards.First());

		if (CommandMachine.instance != null) {
			CommandMachine.instance.Reset();
		}

		Reset();
	}

	public void SetCurrent(Board board) {
		if (current != null) {
			current.gameObject.SetActive(false);
		}
		current = board;
		if (current != null) {
			current.gameObject.SetActive(true);
		}
		text.text = "Test {0}/{1}".i(boards.IndexOf(current)+1, boards.Count);
	}

	public void Start() {
		Generate();
	}

	public void Update() {
		if (!CommandMachine.instance.launched) {
			if (Input.GetButtonDown("Next Test")) {
				SetCurrent(boards.CyclicNext(current));
			}
			if (Input.GetButtonDown("Previous Test")) {
				SetCurrent(boards.CyclicNext(current, -1));
			}
		}
	}

	public void Reset() {
		boards.ForEach(board => board.Reset());
	}

	public void CompleteTest() {
		current.completed = true;
		var nextTest = boards.FirstOrDefault(board => !board.completed);
		if (nextTest == null) {
			CompleteProblem();
		} else {
			SetCurrent(nextTest);
		}
	}

	public void CompleteProblem() {
		Player.instance.Win();
	}
}
