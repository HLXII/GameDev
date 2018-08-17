using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryCanvas : MonoBehaviour {

	// Item prefabs
	public GameObject itemBack;
	public GameObject itemSelectOutline;

	public GameObject item;

	// Dictionary to store item prefab ids
	private Dictionary<string, GameObject> itemDict;

	// References to external GameObjects
	public DataManager dataManager;
	public ItemSelect itemSelect;

	private Transform inventoryPageFilter;
	private Transform inventoryFilter;

	private Transform inventory;
	private Transform inventoryBack;

	private Transform equipLeft;
	private Transform equipRight;
	private Transform toolBar;

	private string pageFilter;

	public string PageFilter { set { pageFilter = value; } }

	// Use this for initialization
	void Start () {

		// Retrieving references to external GameObjects
		dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();
		itemSelect = GameObject.Find ("ItemSelect").GetComponent<ItemSelect> ();

		inventoryPageFilter = GameObject.Find ("InventoryPageFilter").transform;
		inventoryFilter = GameObject.Find ("InventoryFilter").transform;

		inventory = GameObject.Find ("InventoryContent").transform;
		inventoryBack = GameObject.Find ("InventoryBack").transform;

		equipLeft = GameObject.Find ("EquipLeft").transform;
		equipRight = GameObject.Find ("EquipRight").transform;
		toolBar = GameObject.Find ("ToolBar").transform;

		updateInventory ("Consumable");
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	public void exitInventory() {
		SceneManager.LoadScene("Overworld");
	}

	public void updateInventory() {
		
		#region Inventory Section

		// Removing old items
		foreach (Transform child in inventory) {
			GameObject.Destroy(child.gameObject);
		}
		inventory.DetachChildren();
		// Removing rune backs
		foreach (Transform child in inventoryBack) {
			GameObject.Destroy (child.gameObject);
		}
		inventoryBack.DetachChildren();

		List<ItemData> filteredItems = dataManager.Inventory.getItems(pageFilter);

		// Instantiating all filtered items
		foreach (ItemData itemData in filteredItems) {
			GameObject instance = Instantiate (item, new Vector3 (0,0,1), Quaternion.identity,inventory);
			instance.GetComponent<Item> ().ItemData = itemData;
			Instantiate (itemBack, new Vector3 (0, 0, 1), Quaternion.identity, inventoryBack);
		}

		// Updating size of InventoryContent and InventoryBack
		RectTransform content = (RectTransform)inventory.parent.transform;
		content.sizeDelta = new Vector2 (content.rect.size.x, ((filteredItems.Count+6) / 7) * 40 * inventory.localScale.x);

		#endregion

		#region Equip Section

		// Removing old items
		foreach (Transform child in equipLeft) {
			GameObject.Destroy(child.gameObject);
		}
		equipLeft.DetachChildren();
		foreach (Transform child in equipRight) {
			GameObject.Destroy(child.gameObject);
		}
		equipRight.DetachChildren();
		foreach (Transform child in toolBar) {
			GameObject.Destroy(child.gameObject);
		}
		toolBar.DetachChildren();
		ItemData[] equipLeftData = dataManager.Inventory.EquipLeft;
		ItemData[] equipRightData = dataManager.Inventory.EquipRight;
		ItemData[] toolBarData = dataManager.Inventory.ToolBar;

		for (int i = 0; i < 3; i++) {
			GameObject instance = Instantiate (item, new Vector3 (0,0,1), Quaternion.identity,equipLeft);
			instance.GetComponent<Item> ().ItemData = equipLeftData[i];

			instance = Instantiate (item, new Vector3 (0,0,1), Quaternion.identity,equipRight);
			instance.GetComponent<Item> ().ItemData = equipRightData[i];
		}

		for (int i = 0; i < 6; i++) {
			GameObject instance = Instantiate (item, new Vector3 (0,0,1), Quaternion.identity,toolBar);
			instance.GetComponent<Item> ().ItemData = toolBarData[i];
		}

		#endregion

		#region Selection Section

		ItemData selectedItemData = itemSelect.GetComponent<ItemSelect>().ItemData;

		// If there was a item selected
		if (selectedItemData != null) {

			bool found = false;

			Transform[] itemSlots = { inventory, equipLeft, equipRight, toolBar };

			foreach (Transform itemSlot in itemSlots) {

				foreach (Transform child in itemSlot) {

					// If the selected rune exists in the inventory, set it as selected and end
					if (child.gameObject.GetComponent<Item> ().ItemData == selectedItemData) {
						//Debug.Log("Found " + child.gameObject);
						child.gameObject.GetComponent<Item> ().onSelect ();
						found = true;
						break;

					}
				}
					
				if (found) {
					break;
				}
	
			}

			// Rune wasn't found, clear selection
			if (!found) {
				//Debug.Log ("Not Found, clearing Selection");
				itemSelect.GetComponent<ItemSelect> ().clearSelect ();
			}
		}

		#endregion
	}

	public void updateInventory(string pageFilter) {
		this.pageFilter = pageFilter;
		updateInventory ();
	}

}
