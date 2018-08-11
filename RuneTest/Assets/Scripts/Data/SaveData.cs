using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData {

	private string fileName;

	private Inventory inventory;
	private Settings settings;
	private PlayerData player;

	public string FileName { get { return fileName; } }
	public Inventory Inventory { get { return inventory; } }
	public Settings Settings { get { return settings; } }
	public PlayerData Player { get { return player; } }

	public SaveData(string fileName) {

	}

	public SaveData(string fileName, Inventory inventory, Settings settings, PlayerData player) {
		this.fileName = fileName;
		this.inventory = inventory;
		this.settings = settings;
		this.player = player;
	}

}

[System.Serializable]
public class PlayerData {

}

