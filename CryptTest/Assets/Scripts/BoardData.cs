using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public enum BoardType {Title, Editor, Select, Light};

[System.Serializable]
public class BoardData {

	public BoardType boardType;
	public int boardID;
	public int[,] gemMatrix;

	// Use this for initialization
	void Awake () {

		/* Setup Title Board

		Debug.Log ("Setting up Title Board...");

		Load (0);

		puzzle = new Puzzle ();
		puzzle.boardType = BoardType.Title;
		puzzle.gemMatrix = new int[,] { { 1, 1 }, { 1, 1 } };

		for (int i = 0; i < gemMatrix.GetLength (0); i++) {
			for (int j = 0; j < gemMatrix.GetLength (1); j++) {
				gemMatrix [i, j] = Random.Range (0, 2);
			}
		}
		puzzle = new Puzzle ();
		puzzle.boardType = BoardType.Select;
		int[,] gemMatrix = new int[,]   {{8,8,1,1,8,8},
								  {8,8,1,1,8,8},
								  {1,1,1,1,1,1},
								  {1,1,1,1,1,1},
								  {8,8,1,1,8,8},
								  {8,8,1,1,8,8}};
		puzzle.gemMatrix = gemMatrix;
		Save ("10.txt");

		TextAsset test = Resources.Load<TextAsset> ("Light/ind_header");
		Debug.Log (test.name);

		DontDestroyOnLoad (this);*/

	}

	/*public void Setup (string scene) {
		
		// Setting up Light Selection Screen
		if (scene == "Light") {
			puzzle = new Puzzle ();
			puzzle.boardType = BoardType.Select;
			puzzle.gemMatrix = new int[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
			print ("Puzzle Set Up");
		} else {
			Debug.Log ("Invalid Scene Selection");
		}
	}*/

	/*public void Load(int boardID) {

		
		 * 	PuzzleIDs are formatted in this way:
		 * 		0 - 		Title
		 * 		1 - 		Editor
		 *		10-99 - 	Puzzle Types
		 *		100-199 -	Light
		 *		200-299 - 	??
		 *		...
		 *
		 

		Debug.Log ("Loading " + boardID);

		// OSXEditor files
		if (Application.platform == RuntimePlatform.OSXEditor) {

			//Debug.Log ("Loading " + puzzle.boardType + "/" + puzzleID);
			
			BinaryFormatter bf = new BinaryFormatter ();
			TextAsset boardData = Resources.Load<TextAsset> (boardID.ToString());
			Stream s = new MemoryStream(boardData.bytes);
			this = (Board)bf.Deserialize (s);
		
		// IPhonePlayer files
		} else if (Application.platform == RuntimePlatform.IPhonePlayer) {

		} else {
			Debug.Log ("Invalid Platform : " + Application.platform);
		}

	}*/

	/*public void Save(string filename) {
		Debug.Log ("Saving Board at " + Application.persistentDataPath + "/" + filename);
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/" + filename);
		bf.Serialize(file, board);
		file.Close();
	}

	public static string GetiPhoneDocumentsPath () { 
		string path = Application.dataPath.Substring (0, Application.dataPath.Length - 5); 
		path = path.Substring(0, path.LastIndexOf('/'));  
		return path + "/Documents"; 
	}*/

}
