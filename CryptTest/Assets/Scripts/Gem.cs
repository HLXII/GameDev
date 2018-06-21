using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Gem : MonoBehaviour, IPointerClickHandler {

	public static Dictionary<int,Color> colorID = new Dictionary<int, Color> () 
	{   {0,new Color(1,1,1)},																			
		{1,new Color(.25f,.25f,.25f)},
		{2,new Color(1,0,0)},
		{3,new Color(1,.5f,0)},
		{4,new Color(1,1,0)},
		{5,new Color(0,.5f,0)},
		{6,new Color(0,0,1)},
		{7,new Color(1,0,1)},};

	public int gemColor;
	public int rimColor;

	public void setGem(int gemID) {
		gemColor = gemID;
		this.transform.GetChild (0).GetComponent<Image> ().color = colorID [gemID];
		//Debug.Log (this.transform.GetChild (0).GetComponent<Image> ().color);
		//gameObject.GetComponent<Image> ().sprite = gemTypes [gemID];
			//Debug.Log ("Setting " + this.transform.position + " " + gameObject.GetComponent<Image> ().sprite.name);
		/*
		if (checkGem ()) {
			this.transform.GetChild (0).GetComponent<Animator> ().SetTrigger ("Sparkle");
		} else {
			this.transform.GetChild (0).GetComponent<Animator> ().SetTrigger ("Idle");
			Debug.Log ("????");
		}*/

	}

	public void setRim(int rimID) {
		rimColor = rimID;
		this.transform.GetChild (2).GetComponent<Image> ().color = colorID [rimID];
	}

	public bool checkGem() {
		return (rimColor == gemColor);
	}

	public virtual void OnPointerClick (PointerEventData eventData) {
		Debug.Log (this);
	}

	// Button Function for Title Screen
	public void ClickLoadScene() {

		BoardManager boardManager = GameObject.Find ("BoardManager").GetComponent<BoardManager> ();
		Debug.Log ("Loading Scene");

		float sceneID = gameObject.transform.position.x + (2 - gameObject.transform.position.y-1) * 2;

		//Debug.Log ("Scene ID: " + sceneID);

		switch ((int)sceneID) {

		// Puzzle Selection
		case 0:
			Debug.Log ("Scene ID: Select");
			SceneManager.LoadScene ("Select");
			boardManager.LoadBoardData (BoardType.Select, 0);
			break;

		// Puzzle Editor
		case 1:
			Debug.Log ("Scene ID: Editor");
			boardManager.LoadBoardData (BoardType.Editor, 0);
			SceneManager.LoadScene ("Editor");
			break;

		// Created Puzzles
		case 2:
			Debug.Log ("Scene ID: Created Puzzles");
			break;

		// Options
		case 3:
			Debug.Log ("Scene ID: Options");
			break;
		
		default:
			Debug.Log ("How did you get this SceneID?");
			break;
		}
	}

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
	}

	public void ClickLight() {

		Transform board = GameObject.Find ("Board").transform;

		int size = (int)Mathf.Sqrt (board.transform.childCount);

		Vector3 position = this.transform.position;

		//int index = (int) (this.transform.position.x * size + this.transform.position.y);

		//Debug.Log("Position: " + this.transform.position + " Index: " + index);

		/*Vector3 up = this.transform.position;
		up.y = (up.y + 1) % size;
		Vector3 down = this.transform.position;
		down.y = (down.y - 1) % size;
		Vector3 left = this.transform.position;
		left.x = (left.x - 1) % size;
		Vector3 right = this.transform.position;
		right.x = (right.x + 1) % size;

		Debug.Log ("Up: " + up + " Down: " + down + " Left: " + left + " Right: " + right);
		*/
		

		GameObject gemMid = board.GetChild ((int)(position.x * size + position.y)).gameObject;
		GameObject gemUp = board.GetChild ((int)(position.x * size + (position.y + 1) % size)).gameObject;
		GameObject gemDown = board.GetChild ((int)(position.x * size + (position.y + size - 1) % size)).gameObject;
		GameObject gemLeft = board.GetChild ((int)(((position.x + size - 1) % size) * size + position.y)).gameObject;
		GameObject gemRight = board.GetChild ((int)(((position.x + 1) % size) * size + position.y)).gameObject;

		GameObject[] gems = new GameObject[] { gemMid, gemUp, gemDown, gemLeft, gemRight };

		for (int i = 0; i < gems.Length; i++) {
			
			// Gem is Black
			if (gems [i].GetComponent<Gem> ().gemColor == 0) {
				gems [i].GetComponent<Gem> ().setGem (1);
				// Gem is White
			} else {
				gems [i].GetComponent<Gem> ().setGem (0);
			}

		}

		GameObject.Find ("BoardManager").GetComponent<BoardManager> ().CheckComplete ();

	}

}
