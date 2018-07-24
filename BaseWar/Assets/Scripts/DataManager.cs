using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class BoardData {

	private string[,] board;

	// Can't use other types like Rect or Vector4, so have to use arrays
	// Formatted as x, y, w, h
	private int[] playerBounds;
	private int[] enemyBounds;

	public string[,] Board { get { return board; } }
	public int[] PlayerBounds { get { return playerBounds; } }
	public int[] EnemyBounds { get { return enemyBounds; } }

	// Testing Scripts
	public BoardData() {

		int height = 10;
		int width = 20;

		board = new string[height, width];

		for (int h = 0; h < height; h++) {
			for (int w = 0; w < width; w++) {

				if (h > 5) {
					board [h, w] = "Stone";
				} else {
					board [h, w] = "Air";
				}

			}
		}

		playerBounds = new int[] {0, 0, 10, 10};
		enemyBounds = new int[] {10, 0, 10, 10};

	}

}

public class DataManager : MonoBehaviour {

	private string boardName;

	// Use this for initialization
	void Start () {

		// Creating one instance of this gameObject
		DontDestroyOnLoad (this);
		if (FindObjectsOfType(GetType()).Length > 1)
		{
			Destroy(gameObject);
		}

		// Checking PlayerPrefs
		if (!PlayerPrefs.HasKey ("Exists")) {
			Debug.Log ("Creating New PlayerPrefs");
			initPlayerPrefs ();
		} else {
			Debug.Log ("PlayerPrefs Exist");
		}

		BoardData t = new BoardData ();

		Save ("test", t);

		// Testing scripts
		boardName = "test";

	}

	// Initializes PlayerPrefs
	public void initPlayerPrefs() {
		// Initalization settings
		//***
		// Storing PlayerPrefs
		PlayerPrefs.Save ();
	}

	public void Save(string filename, BoardData board) {
		filename += ".txt";
		Debug.Log ("Saving Board at " + Application.persistentDataPath + "/" + filename);
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/" + filename);
		bf.Serialize(file, board);
		file.Close();
	}

	public BoardData LoadBoard() {
		Debug.Log ("Loading " + boardName);
		
		BinaryFormatter bf = new BinaryFormatter ();
		TextAsset boardFile = Resources.Load<TextAsset> (boardName);
		Stream s = new MemoryStream(boardFile.bytes);
		return (BoardData)bf.Deserialize (s);
	}

}
