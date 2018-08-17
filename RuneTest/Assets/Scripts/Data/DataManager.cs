using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataManager : MonoBehaviour {

	// Static Data Management
	public List<ItemTemplate> itemTemplates;
	public static Dictionary<string, ItemTemplate> item;

	// Main Data objects
	[SerializeField]
	private Inventory inventory;
	private PlayerData player;

	// For data between scenes
	private TableData tableData;
	private PageData pageData;

	public Inventory Inventory { get { return inventory; } }
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

		item = new Dictionary<string, ItemTemplate> ();
		foreach (ItemTemplate itemTemplate in itemTemplates) {
			item.Add (itemTemplate.id, itemTemplate);
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void createNewSave() {

		// Initializing data
		inventory = new Inventory();
		player = new PlayerData ();

		// Initializing scene transition data just in case of bugs
		tableData = new TableData();
		pageData = new PageData ();

	}

	public void load(string filePath) {

		Debug.Log ("Loading Data at " + filePath);

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream dataFile = File.Open(filePath, FileMode.Open);
		SaveData save = (SaveData)bf.Deserialize (dataFile);
		dataFile.Close();

		inventory = save.Inventory;
		player = save.Player;

	}

	public void save(string fileName) {

		SaveData save = new SaveData (fileName, inventory, player);

		Debug.Log ("Saving Data at " + Application.persistentDataPath + "/saves/" + fileName);

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/saves/" + fileName);
		bf.Serialize(file, save);
		file.Close();

	}

}
