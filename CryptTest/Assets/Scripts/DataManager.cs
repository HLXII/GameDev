using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class BoardData {

	public int[,,] board;

}

public class DataManager : MonoBehaviour {

	private BoardData board;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this);

		if (FindObjectsOfType(GetType()).Length > 1)
		{
			Destroy(gameObject);
		}

		if (!PlayerPrefs.HasKey ("Exists")) {
			Debug.Log ("Creating new PlayerPrefs");
			PlayerPrefs.SetInt ("Exists", 0);
			PlayerPrefs.SetInt ("LightSelect", 0);
			PlayerPrefs.SetInt ("Light0",0);

			PlayerPrefs.Save ();
		} else {
			Debug.Log ("PlayerPrefs exist");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
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
