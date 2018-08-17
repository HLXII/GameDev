using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData {

	[SerializeField]
	private string itemTemplate;
	public string ItemTemplate { get { return itemTemplate; } }

	public ItemData(string itemTemplate) {
		this.itemTemplate = itemTemplate;
	}

}
