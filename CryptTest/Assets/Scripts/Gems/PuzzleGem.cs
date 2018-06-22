using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PuzzleGem : Gem {

	public enum Puzzle {Lights, RowsAndColumns, InvertDrag, Switch3, Rotate3, SwitchDrag};

	private Puzzle puzzleId = 0;
	public Puzzle PuzzleId {get{return puzzleId;} set{puzzleId = value;}}

	public override void OnPointerClick (PointerEventData eventData) {
		Debug.Log ("Puzzle ID: " + puzzleId.ToString());

		SceneManager.LoadScene (puzzleId.ToString ());
	}
		
	/*
	// Button Function for Selection Screen
	public void ClickLoadPuzzle() {

		Debug.Log ("Loading Puzzle");

		BoardManager boardManager = GameObject.Find ("BoardManager").GetComponent<BoardManager> ();
		// Use board type to find current puzzle selection screen
		BoardData boardData = boardManager.getBoardData();
		BoardType puzzleType = (BoardType)((int)boardData.boardType + 1);


		Vector3 position = this.transform.position;
		int size = boardData.gemMatrix.GetLength(0);

		int puzzleID = (int) (position.x + (size - position.y - 1) * size);

		Debug.Log ("Puzzle ID: " + puzzleType.ToString() + puzzleID.ToString());

		boardManager.LoadBoardData (puzzleType, puzzleID);
		SceneManager.LoadScene ("Board");
	}*/

}
