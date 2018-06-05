using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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

	public void loadData(string filename) {
		//Debug.Log ("Loading Data: " + dataType + ", " + dataName);
		buildData = new BuildData ();
		saveData ("test4x4.txt");
		/*
		// OSXEditor files
		if (Application.platform == RuntimePlatform.OSXEditor) {


			//TextAsset[] test = Resources.FindObjectsOfTypeAll<TextAsset> ();
			//for (int i = 0; i < test.Length; i++) {
			//	Debug.Log (test [i].name);
			//}

			//Debug.Log ("Loading " + puzzle.boardType + "/" + puzzleID);

			//Debug.Log ("Loading " + filename);

			BinaryFormatter bf = new BinaryFormatter ();
			TextAsset dataFile = Resources.Load<TextAsset> (filename);
			Stream s = new MemoryStream (dataFile.bytes);
			buildData = (BuildData)bf.Deserialize (s);

			// IPhonePlayer files
		} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
			Debug.Log ("Loading " + filename);

			BinaryFormatter bf = new BinaryFormatter ();
			TextAsset boardFile = Resources.Load<TextAsset> (filename);
			Stream s = new MemoryStream (boardFile.bytes);
			buildData = (BuildData)bf.Deserialize (s);
		} else {
			Debug.Log ("Invalid Platform : " + Application.platform);
		}*/
			
	}

	public void saveData(string filename) {
		Debug.Log ("Saving Data at " + Application.persistentDataPath + "/" + filename);
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/" + filename);
		bf.Serialize(file, buildData);
		file.Close();
	}

}
