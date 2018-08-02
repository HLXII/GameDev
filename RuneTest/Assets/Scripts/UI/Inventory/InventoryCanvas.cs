using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryCanvas : MonoBehaviour {

	// Item prefabs
	public GameObject itemBack;
	public GameObject itemSelectOutline;
	public GameObject itemEmpty;

	public GameObject Chili;

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

		// Initializing item dictionary
		initItemDict ();

		initItems ();



	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void initItemDict() {
		itemDict = new Dictionary<string, GameObject> () {
			{"Empty Item",itemEmpty},

			{"Chili",Chili}
		};
	}

	private void initItems() {

		updateEquips ();

		updateInventory ("Consumable");
	}
		
	public void exitInventory() {
		SceneManager.LoadScene("Overworld");
	}

	public void updateEquips() {

		// Removing old items
		foreach (Transform child in equipLeft) {
			GameObject.Destroy(child.gameObject);
		}
		foreach (Transform child in equipRight) {
			GameObject.Destroy(child.gameObject);
		}
		foreach (Transform child in toolBar) {
			GameObject.Destroy(child.gameObject);
		}

		ItemData[] equipLeftData = dataManager.Inventory.EquipLeft;
		ItemData[] equipRightData = dataManager.Inventory.EquipRight;
		ItemData[] toolBarData = dataManager.Inventory.ToolBar;

		for (int i = 0; i < 3; i++) {
			GameObject instance = Instantiate (itemDict [equipLeftData[i].Id],new Vector3 (0,0,1), Quaternion.identity,equipLeft);
			instance.GetComponent<Item> ().ItemData = equipLeftData[i];
			instance.layer = LayerMask.NameToLayer("Items");

			instance = Instantiate (itemDict [equipRightData[i].Id],new Vector3 (0,0,1), Quaternion.identity,equipRight);
			instance.GetComponent<Item> ().ItemData = equipRightData[i];
			instance.layer = LayerMask.NameToLayer("Items");
		}

		for (int i = 0; i < 6; i++) {
			GameObject instance = Instantiate (itemDict [toolBarData[i].Id],new Vector3 (0,0,1), Quaternion.identity,toolBar);
			instance.GetComponent<Item> ().ItemData = toolBarData[i];
			instance.layer = LayerMask.NameToLayer("Items");
		}
	}

	public void updateInventory() {
		// Removing old items
		foreach (Transform child in inventory) {
			GameObject.Destroy(child.gameObject);
		}
		// Removing rune backs
		foreach (Transform child in inventoryBack) {
			GameObject.Destroy (child.gameObject);
		}

		List<ItemData> filteredItems = dataManager.Inventory.getItems(pageFilter);

		// Instantiating all filtered items
		foreach (ItemData item in filteredItems) {
			GameObject instance = Instantiate (itemDict [item.Id],new Vector3 (0,0,1), Quaternion.identity,inventory);
			instance.GetComponent<Item> ().ItemData = item;
			instance.layer = LayerMask.NameToLayer("Items");
			Instantiate (itemBack, new Vector3 (0, 0, 1), Quaternion.identity, inventoryBack);
		}

		// Updating size of InventoryContent and InventoryBack
		RectTransform content = (RectTransform)inventory.parent.transform;
		content.sizeDelta = new Vector2 (content.rect.size.x, ((filteredItems.Count+6) / 7) * 40 * inventory.localScale.x);

	}

	public void updateInventory(string pageFilter) {
		this.pageFilter = pageFilter;
		updateInventory ();
	}

}
