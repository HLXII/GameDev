using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataManager : MonoBehaviour {

	// Old
	//private BuildData buildData;

	// Save Data (Might need to make into one class for easy storage)
	private Inventory inventory;
	//private PlayerData playerData;

	// For data between scenes
	private TableData tableData;
	private PageData pageData;

	public Inventory Inventory { get { return inventory; } }
	public TableData TableData { get { return tableData; } }
	public PageData PageData { get { return pageData; } }

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this);

		if (FindObjectsOfType(GetType()).Length > 1)
		{
			Destroy(gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void loadSave(int saveID) {

		// Temporary
		inventory = new Inventory();
		tableData = new TableData();
		pageData = new PageData ();


		// read save ID, search for files, load inventory, prefs, etc

	}

//
//	public void loadData(string filename) {
//		//Debug.Log ("Loading Data: " + dataType + ", " + dataName);
//		buildData = new BuildData (5,7);
//		saveData ("test4x4.txt");
//		/*
//		// OSXEditor files
//		if (Application.platform == RuntimePlatform.OSXEditor) {
//
//
//			//TextAsset[] test = Resources.FindObjectsOfTypeAll<TextAsset> ();
//			//for (int i = 0; i < test.Length; i++) {
//			//	Debug.Log (test [i].name);
//			//}
//
//			//Debug.Log ("Loading " + puzzle.boardType + "/" + puzzleID);
//
//			//Debug.Log ("Loading " + filename);
//
//			BinaryFormatter bf = new BinaryFormatter ();
//			TextAsset dataFile = Resources.Load<TextAsset> (filename);
//			Stream s = new MemoryStream (dataFile.bytes);
//			buildData = (BuildData)bf.Deserialize (s);
//
//			// IPhonePlayer files
//		} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
//			Debug.Log ("Loading " + filename);
//
//			BinaryFormatter bf = new BinaryFormatter ();
//			TextAsset boardFile = Resources.Load<TextAsset> (filename);
//			Stream s = new MemoryStream (boardFile.bytes);
//			buildData = (BuildData)bf.Deserialize (s);
//		} else {
//			Debug.Log ("Invalid Platform : " + Application.platform);
//		}*/
//			
//	}
//
//	public void saveData(string filename) {
//		Debug.Log ("Saving Data at " + Application.persistentDataPath + "/" + filename);
//		BinaryFormatter bf = new BinaryFormatter();
//		FileStream file = File.Create (Application.persistentDataPath + "/" + filename);
//		bf.Serialize(file, buildData);
//		file.Close();
//	}

}
