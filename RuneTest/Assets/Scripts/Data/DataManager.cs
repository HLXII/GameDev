using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataManager : MonoBehaviour {

	// Main Data objects
	private Inventory inventory;
	private Settings settings;
	private PlayerData player;

	// For data between scenes
	private TableData tableData;
	private PageData pageData;

	public Inventory Inventory { get { return inventory; } }
	public Settings Settings { get { return settings; } }
	public PlayerData Player { get { return player; } }

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

		save ("save1.sav");

	}

	public void load(string fileName) {

		Debug.Log ("Loading Data at " + Application.persistentDataPath + "/saves/" + fileName);

		BinaryFormatter bf = new BinaryFormatter ();
		TextAsset dataFile = Resources.Load<TextAsset> (Application.persistentDataPath + "/saves/" + fileName);
		Stream s = new MemoryStream (dataFile.bytes);
		SaveData save = (SaveData)bf.Deserialize (s);

		inventory = save.Inventory;
		settings = save.Settings;
		player = save.Player;

	}

	public void save(string fileName) {

		SaveData save = new SaveData (fileName, inventory, settings, player);

		Debug.Log ("Saving Data at " + Application.persistentDataPath + "/saves/" + fileName);

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/saves/" + fileName);
		bf.Serialize(file, save);
		file.Close();

	}

}
