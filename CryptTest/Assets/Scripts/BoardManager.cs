using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class BoardManager : MonoBehaviour {

	private Transform board;

	private BoardData boardData;

	public GameObject Gem;

	void Start () {

		DontDestroyOnLoad (this);

		LoadBoardData (BoardType.Title, 0);
		InitBoard (SceneManager.GetActiveScene(),SceneManager.GetActiveScene());

		SceneManager.activeSceneChanged += InitBoard;

	}

	/*public void Setup(BoardType boardType, int boardID) {

		/*boardData = new BoardData ();
		boardData.boardType = BoardType.Title;
		boardData.boardID = 0;
		boardData.gemMatrix = new int[,] { { 16, 48 }, { 32, 40 } };
		SaveBoardData ("Title0.txt");*/

		/*boardData = new BoardData ();
		boardData.boardType = BoardType.Select;
		int[,] gemMatrix = new int[,]   {{8,8,1,1,8,8},
			{8,8,1,1,8,8},
			{1,1,1,1,1,1},
			{1,1,1,1,1,1},
			{8,8,1,1,8,8},
			{8,8,1,1,8,8}};
		boardData.gemMatrix = gemMatrix;
		SaveBoardData ("Select0.txt");*/

		/*boardData = new BoardData ();
		boardData.boardType = BoardType.Light;
		int[,] gemMatrix = new int[,] { { 0, 8, 0 }, { 8, 8, 8 }, { 0, 8, 0 } };
		boardData.gemMatrix = gemMatrix;
		SaveBoardData ("Light0.txt");

		LoadBoardData (boardType, boardID);

		InitBoard (SceneManager.GetActiveScene(),SceneManager.GetActiveScene());

	}*/

	public void LoadBoardData(BoardType boardtype, int boardID) {

		// OSXEditor files
		if (Application.platform == RuntimePlatform.OSXEditor) {

			/*TextAsset[] test = Resources.FindObjectsOfTypeAll<TextAsset> ();
			for (int i = 0; i < test.Length; i++) {
				Debug.Log (test[i].name);
			}*/

			//Debug.Log ("Loading " + puzzle.boardType + "/" + puzzleID);

			string filename = boardtype.ToString () + boardID.ToString();
			Debug.Log ("Loading " + filename);

			BinaryFormatter bf = new BinaryFormatter ();
			TextAsset boardFile = Resources.Load<TextAsset> (filename);
			Stream s = new MemoryStream(boardFile.bytes);
			boardData = (BoardData)bf.Deserialize (s);

			// IPhonePlayer files
		} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
			string filename = boardtype.ToString () + boardID.ToString();
			Debug.Log ("Loading " + filename);

			BinaryFormatter bf = new BinaryFormatter ();
			TextAsset boardFile = Resources.Load<TextAsset> (filename);
			Stream s = new MemoryStream(boardFile.bytes);
			boardData = (BoardData)bf.Deserialize (s);
		} else {
			Debug.Log ("Invalid Platform : " + Application.platform);
		}



	}

	private void InitBoard(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.Scene arg1) {

		Debug.Log ("Initializing Board");

		if (GameObject.Find ("Board") != null) {
			Destroy (GameObject.Find ("Board"));
		}

		Debug.Log (Gem);

		board = new GameObject ("Board").transform;

		for (int i = 0; i < boardData.gemMatrix.GetLength(0); i++) {
			for (int j = 0; j < boardData.gemMatrix.GetLength(0); j++) {

				GameObject instance = Instantiate (Gem, new Vector3 (i, j, 0F), Quaternion.identity) as GameObject;

				int rimID = boardData.gemMatrix [i, j] % 8;
				int gemID = (int)((boardData.gemMatrix [i, j] - rimID) / 8);

				Debug.Log("ID: " + boardData.gemMatrix[i,j] + " Gem: " + gemID + " Rim: " + rimID);

				instance.GetComponent<Gem> ().setGem (gemID);
				instance.GetComponent<Gem> ().setRim (rimID);

				switch (boardData.boardType) {
				case BoardType.Title:
					instance.transform.GetChild(2).GetComponent<Button> ().onClick.AddListener (instance.GetComponent<Gem> ().ClickLoadScene);
					break;
				case BoardType.Select:
					instance.transform.GetChild(2).GetComponent<Button> ().onClick.AddListener (instance.GetComponent<Gem> ().ClickLoadPuzzle);
					break;
				case BoardType.Editor:
					break;
				case BoardType.Light:
					instance.transform.GetChild(2).GetComponent<Button> ().onClick.AddListener (instance.GetComponent<Gem> ().ClickLight);
					break;
				default:
					break;
				}

				instance.transform.SetParent (board);
			}
		}
		
		board.SetParent (gameObject.transform);

		GameObject.Find ("Main Camera").transform.position = new Vector3 ((boardData.gemMatrix.GetLength(0)-1)/2f,(boardData.gemMatrix.GetLength(0)-1)/2f,-10f);
		GameObject.Find ("Main Camera").GetComponent<Camera> ().orthographicSize = Mathf.Max(boardData.gemMatrix.GetLength (0),4);

	}

	public void CheckComplete() {

		Debug.Log ("Checking if board is complete");

		for (int i = 0; i < board.transform.childCount; i++) {
			if (!board.transform.GetChild (i).GetComponent<Gem> ().checkGem ()) {
				Debug.Log ("Not Complete");
				return;
			}
		}

		Debug.Log ("Complete!");
		//Animation
		for (int i = 0; i < board.transform.childCount; i++) {
			board.transform.GetChild (i).GetComponent<Gem> ().transform.GetChild (0).GetComponent<Animator> ().SetTrigger ("Sparkle");
		}
	}

	private void SaveBoardData(string filename) {
		Debug.Log ("Saving Board at " + Application.persistentDataPath + "/" + filename);
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/" + filename);
		bf.Serialize(file, boardData);
		file.Close();
	}

	public BoardData getBoardData() {
		return boardData;
	}
		
}
