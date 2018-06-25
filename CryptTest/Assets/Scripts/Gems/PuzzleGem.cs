using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PuzzleGem : Gem {

	private int puzzleId = 0;
	public int PuzzleId {get{return puzzleId;} set{puzzleId = value;}}

	private string puzzleType = "";
	public string PuzzleType { get { return puzzleType; } set { puzzleType = value; } }

	public override void OnPointerClick (PointerEventData eventData) {

		if (!(gemColor == 1 && rimColor == 1)) {

			Debug.Log ("Puzzle: " + puzzleType + " " + puzzleId.ToString ());

			GameObject.Find ("DataManager").GetComponent<DataManager> ().loadPuzzle (puzzleType, puzzleId);

			SceneManager.LoadScene (puzzleType + "Board");

		}
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
