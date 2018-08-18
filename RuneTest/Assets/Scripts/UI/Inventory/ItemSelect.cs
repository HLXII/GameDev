using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelect : MonoBehaviour {

	private GameObject item;
	private ItemData itemData;

	public GameObject Item { get { return item; }
		set {
			if (item != null) {
				item.GetComponent<Item> ().deSelect ();
			}
			item = value; 
			itemData = item.GetComponent<Item> ().ItemData;

			updateSelect ();

		} 
	}
	public ItemData ItemData { get { return itemData; } }

	private GameObject itemImage;
	private GameObject itemText;
	private GameObject useButton;
	private GameObject runeButton;

	// Use this for initialization
	void Start () {
		item = null;
		itemData = null;

		itemImage = transform.GetChild (0).gameObject;
		itemText = transform.GetChild (1).gameObject;

		useButton = transform.GetChild (2).GetChild (0).gameObject;
		runeButton = transform.GetChild (2).GetChild (1).gameObject;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Escape)) {
			clearSelect ();
		}

	}

	public void updateSelect() {
		ItemTemplate itemTemplate = DataManager.item [itemData.ItemTemplate];

		itemImage.GetComponent<Image> ().sprite = itemTemplate.icon;
		itemText.GetComponent<Text> ().text = itemTemplate.description;

		useButton.GetComponent<Button> ().enabled = itemTemplate.isConsumable;
		runeButton.GetComponent<Button> ().enabled = itemTemplate.isRuneable;
	}

	public void clearSelect() {
		//Debug.Log ("Clear Selection");

		itemImage.GetComponent<Image> ().sprite = null;
		itemText.GetComponent<Text> ().text = "";
		if (item != null) {
			item.GetComponent<Item> ().deSelect ();
			item = null;
			itemData = null;
		}


		useButton.GetComponent<Button> ().enabled = false;
		runeButton.GetComponent<Button> ().enabled = false;


	}
}
