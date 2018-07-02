using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class BoardData {

	public int[,,] matrix;

	public BoardData(int[,,] data) {
		matrix = data;
	}

}

public class DataManager : MonoBehaviour {

	private BoardData board;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this);

		int[,,] data = new int[,,] {
			{{1,1},{1,1},{1,1}},
			{{1,1},{1,1},{1,1}},
			{{1,1},{1,1},{1,1}}
		};

		board = new BoardData (data);

		Save ("Light0.txt");

		if (FindObjectsOfType(GetType()).Length > 1)
		{
			Destroy(gameObject);
		}

		if (!PlayerPrefs.HasKey ("Exists")) {
			Debug.Log ("Creating New PlayerPrefs");
			PlayerPrefs.SetInt ("Exists", 0);
			PlayerPrefs.SetInt ("LightSelect", 0);
			PlayerPrefs.SetInt ("Light0",0);

			PlayerPrefs.Save ();
		} else {
			Debug.Log ("PlayerPrefs Exist");
		}
	}

	public int[,,] getData() {
		return board.matrix;
	}

	public void Save(string filename) {
		Debug.Log ("Saving Board at " + Application.persistentDataPath + "/" + filename);
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/" + filename);
		bf.Serialize(file, board);
		file.Close();
	}

	public void loadPuzzle(string puzzleType, int puzzleId) {
		if (Application.platform == RuntimePlatform.OSXEditor) {

			/*TextAsset[] test = Resources.FindObjectsOfTypeAll<TextAsset> ();
			for (int i = 0; i < test.Length; i++) {
				Debug.Log (test[i].name);
			}*/

			//Debug.Log ("Loading " + puzzle.boardType + "/" + puzzleID);

			string filename = puzzleType + puzzleId.ToString();
			Debug.Log ("Loading " + filename);

			BinaryFormatter bf = new BinaryFormatter ();
			TextAsset boardFile = Resources.Load<TextAsset> (filename);
			Stream s = new MemoryStream(boardFile.bytes);
			board = (BoardData)bf.Deserialize (s);

			// IPhonePlayer files
		} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
			string filename = puzzleType + puzzleId.ToString();
			Debug.Log ("Loading " + filename);

			BinaryFormatter bf = new BinaryFormatter ();
			TextAsset boardFile = Resources.Load<TextAsset> (filename);
			Stream s = new MemoryStream(boardFile.bytes);
			board = (BoardData)bf.Deserialize (s);
		} else {
			Debug.Log ("Invalid Platform : " + Application.platform);
		}
	}
		
}
