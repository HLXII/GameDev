using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataManager : MonoBehaviour {

	// Static Data Management
	public List<ItemTemplate> itemTemplates;
	public static Dictionary<string, ItemTemplate> item;

    public List<RuneTemplate> runeTemplates;
    public static Dictionary<string, RuneTemplate> rune;

	// Main Data objects
	private Inventory inventory;
	public Inventory Inventory { get { return inventory; } }
	private PlayerData player;
	public PlayerData Player { get { return player; } }

	// For data between scenes
	private string lastScene;
	public string LastScene { get { return lastScene; } set { lastScene = value; } }

	private TableData tableData;
	public TableData TableData { get { return tableData; } set { tableData = value; } }
	private PageData pageData;
	public PageData PageData { get { return pageData; } set { pageData = value; } }

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this);

		if (FindObjectsOfType(GetType()).Length > 1)
		{
			Destroy(gameObject);
		}

        item = new Dictionary<string, ItemTemplate>();
		foreach (ItemTemplate itemTemplate in itemTemplates) {
			item.Add(itemTemplate.id, itemTemplate);
		}

        rune = new Dictionary<string, RuneTemplate>();
        foreach (RuneTemplate runeTemplate in runeTemplates) {
            rune.Add(runeTemplate.id, runeTemplate);
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CreateNewSave() {

        Debug.Log("Creating new save");

		// Initializing data
		inventory = new Inventory(1);
		player = new PlayerData ();

		// Initializing scene transition data just in case of bugs
		tableData = new TableData();
		pageData = new PageData ();

	}

	public void Load(string filePath) {

		Debug.Log ("Loading Data at " + filePath);

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream dataFile = File.Open(filePath, FileMode.Open);
		SaveData loadedSave = (SaveData)bf.Deserialize (dataFile);
		dataFile.Close();

		inventory = loadedSave.Inventory;
		player = loadedSave.Player;

	}

	public void Save(string fileName) {

		SaveData currentSave = new SaveData (fileName, inventory, player);

		Debug.Log ("Saving Data at " + Application.persistentDataPath + "/saves/" + fileName);

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/saves/" + fileName);
        bf.Serialize(file, currentSave);
		file.Close();

	}

}
