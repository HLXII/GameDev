using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour {

	private BuildData buildData;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public BuildData getBuildData() {
		return buildData;
	}

	public void loadData(string dataType, string dataName) {
		Debug.Log ("Loading Data: " + dataType + ", " + dataName);
		this.buildData = new BuildData ();
		this.buildData.saveData ("test4x4");
		/*

		// OSXEditor files
		if (Application.platform == RuntimePlatform.OSXEditor) {

			TextAsset[] test = Resources.FindObjectsOfTypeAll<TextAsset> ();
			for (int i = 0; i < test.Length; i++) {
				Debug.Log (test[i].name);
			}

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

		*/
	}

}
