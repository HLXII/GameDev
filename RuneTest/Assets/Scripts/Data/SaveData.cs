using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData {

	private string fileName;

	private Inventory inventory;
	private PlayerData player;

	public string FileName { get { return fileName; } }
	public Inventory Inventory { get { return inventory; } }
	public PlayerData Player { get { return player; } }

	public SaveData() {

	}

	public SaveData(string fileName, Inventory inventory, PlayerData player) {
		this.fileName = fileName;
		this.inventory = inventory;
		this.player = player;
	}

}

[System.Serializable]
public class PlayerData {

}

