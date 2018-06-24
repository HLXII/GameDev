using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneGem : Gem {

	public enum Scene {Title, PuzzleSelect, Options, Delete, 
		LightSelect, RowsAndColumnsSelect,  InvertDragSelect, Switch3Select, Rotate3Select, SwitchDragSelect,
		LightBoard, RowsAndColumnsBoard, InvertDragBoard, Switch3Board, Rotate3Board, SwitchDragBoard};

	private Scene sceneId = Scene.Title;
	public Scene SceneId {get{return sceneId;} set{sceneId = value;}}

	public override void OnPointerClick (PointerEventData eventData) {
		if (gemColor == 1 && rimColor == 1) {
		
		} else {
			Debug.Log ("Scene ID: " + sceneId);

			SceneManager.LoadScene (SceneId.ToString ());
		}

	}


	/*
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
	}*/

}
