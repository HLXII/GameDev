using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHUD : MonoBehaviour {

	private ItemData itemData;

	// Use this for initialization
	void Start () {

		itemData = GameObject.Find ("DataManager").GetComponent<DataManager> ().Inventory.ToolBar [transform.GetSiblingIndex ()];

		transform.GetChild (0).GetComponent<Image> ().sprite = DataManager.item [itemData.ItemTemplate].icon;

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}


