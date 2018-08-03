using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler {

	protected ItemData itemData;

	private bool selected;

	// References to external objects
	private DataManager dataManager;
	private Transform canvas;
	private RectTransform equipLeft;
	private RectTransform equipRight;
	private RectTransform toolBar;
	private RectTransform inventory;

	private InventoryCanvas inventoryCanvas;

	// Bounds of item slots
	private Bounds inventoryBounds;

	private GameObject previous_parent;
	private int previous_index;

	public ItemData ItemData { get { return itemData; } set { itemData = value; } }

	public bool Selected { get { return selected; } 
		set { selected = value;
			if (value) {
				if (canvas == null) {
					canvas = GameObject.Find ("Canvas").transform;
				}
				Instantiate (canvas.GetComponent<InventoryCanvas> ().itemSelectOutline, transform);
			} else {
				if (transform.childCount > 0) {
					for (int i = transform.childCount-1; i >= 0; i--) {
						Destroy (transform.GetChild (i).gameObject);
					}
				}
			} 
		} 
	}

	// Use this for initialization
	void Start () {

		dataManager = GameObject.Find ("DataManager").GetComponent<DataManager> ();
		canvas = GameObject.Find ("Canvas").transform;
		equipLeft = (RectTransform)GameObject.Find ("EquipLeft").transform;
		equipRight = (RectTransform)GameObject.Find ("EquipRight").transform;
		toolBar = (RectTransform)GameObject.Find ("ToolBar").transform;
		inventory = (RectTransform)GameObject.Find ("Inventory").transform;

		inventoryCanvas = canvas.GetComponent<InventoryCanvas> ();

		inventoryBounds = new Bounds (inventory.localPosition, inventory.rect.size);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void drop() {

		RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero, Mathf.Infinity, 1 << LayerMask.NameToLayer("Items"));

		//hit.collider != null && hit.collider.gameObject.transform.parent.name == "PageContent"

		// Dropped onto item slot
		if (hit.collider != null) {

			GameObject old_item = hit.collider.gameObject;
			GameObject new_parent = hit.collider.gameObject.transform.parent.gameObject;
			int new_index = hit.collider.gameObject.transform.GetSiblingIndex ();

			Debug.Log ("Came from " + previous_parent + " at index " + previous_index);

			Debug.Log ("Dropped onto " + new_parent + " at index " + new_index);

			// Determining original location
			switch (previous_parent.name) {
			// Came from Inventory
			case "InventoryContent":

				switch (new_parent.name) {
				// Dropped back onto Inventory, return to original position
				case "InventoryContent":
					returnToOriginalPosition ();
					break;
					// Dropped onto EquipLeft or EquipRight, check if can drop
				case "EquipLeft":
				case "EquipRight":
					// Checking if can drop item onto equips
					if (droppable (itemData, new_index)) {

						// Moving old item to inventory if exists
						dataManager.Inventory.addItem (old_item.GetComponent<Item>().ItemData);

						// Move item to new parent
						dataManager.Inventory.equipItem (itemData, new_parent.name, new_index);
						dataManager.Inventory.removeItem (itemData);

						Destroy (gameObject);

						inventoryCanvas.updateInventory ();

					} else {
						returnToOriginalPosition ();
					}
					break;
					// Dropped onto ToolBar, check if can drop
				case "ToolBar":

					// Moving old item to inventory if exists
					dataManager.Inventory.addItem (old_item.GetComponent<Item> ().ItemData);

					// Move item to new parent
					dataManager.Inventory.equipItem (itemData, new_parent.name, new_index);
					dataManager.Inventory.removeItem (itemData);

					Destroy (gameObject);

					inventoryCanvas.updateInventory ();	

					break;
				}

				break;
			// Came from EquipLeft
			case "EquipLeft":

				switch (new_parent.name) {
				// Dropped into Inventory
				case "InventoryContent":
					// Sending item to inventory
					dataManager.Inventory.addItem (itemData);
					dataManager.Inventory.equipItem (new EmptyItemData (), previous_parent.name, previous_index);

					Destroy (gameObject);

					inventoryCanvas.updateInventory ();

					break;
				// Dropped back onto EquipLeft, return to original position
				case "EquipLeft":
					returnToOriginalPosition ();
					break;
				// Dropped onto EquipRight, check if valid move
				case "EquipRight":
					// If the item types are the same
					if (previous_index == new_index) {

						// Swapping equips
						dataManager.Inventory.equipItem (old_item.GetComponent<Item>().ItemData, previous_parent.name, previous_index);
						dataManager.Inventory.equipItem (gameObject.GetComponent<Item>().ItemData, new_parent.name, new_index);

						Destroy (gameObject);

						inventoryCanvas.updateInventory ();

					} else {
						// Item types different, send to inventory

						// Sending item to inventory
						dataManager.Inventory.addItem (itemData);
						dataManager.Inventory.equipItem (new EmptyItemData (), previous_parent.name, previous_index);

						Destroy (gameObject);

						inventoryCanvas.updateInventory ();

					}
					break;
				// Dropped onto ToolBar, move to toolbar, and check if item can be swapped
				case "ToolBar":

					// Sending item to toolBar
					dataManager.Inventory.equipItem (new EmptyItemData (), previous_parent.name, previous_index);
					dataManager.Inventory.equipItem (itemData, new_parent.name, new_index);

					// If swapped item can be equipped
					if (droppable (old_item.GetComponent<Item> ().itemData, previous_index)) {
						// Send to old position
						dataManager.Inventory.equipItem (old_item.GetComponent<Item> ().ItemData, previous_parent.name, previous_index);
					} else {
						// Send to inventory
						dataManager.Inventory.addItem (old_item.GetComponent<Item> ().ItemData);
					}

					Destroy (gameObject);

					inventoryCanvas.updateInventory ();

					break;
				}

				break;
			// Came from EquipRight
			case "EquipRight":

				switch (new_parent.name) {
				// Dropped into Inventory
				case "InventoryContent":
					//Sending item to inventory
					dataManager.Inventory.addItem (itemData);
					dataManager.Inventory.equipItem (new EmptyItemData (), previous_parent.name, previous_index);

					Destroy (gameObject);

					inventoryCanvas.updateInventory ();
					break;
					// Dropped onto EquipLeft, check if valid move
				case "EquipLeft":
					// If the item types are the same
					if (previous_index == new_index) {

						// Swapping equips
						dataManager.Inventory.equipItem (old_item.GetComponent<Item>().ItemData, previous_parent.name, previous_index);
						dataManager.Inventory.equipItem (gameObject.GetComponent<Item>().ItemData, new_parent.name, new_index);

						Destroy (gameObject);

						inventoryCanvas.updateInventory ();

					} else {
						// Item types different, send to inventory

						// Sending item to inventory
						dataManager.Inventory.addItem (itemData);
						dataManager.Inventory.equipItem (new EmptyItemData (), previous_parent.name, previous_index);

						Destroy (gameObject);

						inventoryCanvas.updateInventory ();

					}
					break;
					// Dropped back onto EquipRight, return to original position
				case "EquipRight":
					returnToOriginalPosition ();
					break;
					// Dropped onto ToolBar, move to toolbar, and check if item can be swapped
				case "ToolBar":

					// Sending item to toolBar
					dataManager.Inventory.equipItem (new EmptyItemData (), previous_parent.name, previous_index);
					dataManager.Inventory.equipItem (itemData, new_parent.name, new_index);

					// If swapped item can be equipped
					if (droppable (old_item.GetComponent<Item> ().itemData, previous_index)) {
						// Send to old position
						dataManager.Inventory.equipItem (old_item.GetComponent<Item> ().ItemData, previous_parent.name, previous_index);
					} else {
						// Send to inventory
						dataManager.Inventory.addItem (old_item.GetComponent<Item> ().ItemData);
					}

					Destroy (gameObject);

					inventoryCanvas.updateInventory ();

					break;
				}

				break;
			// Came from ToolBar
			case "ToolBar":

				switch (new_parent.name) {
				// Dropped into Inventory
				case "InventoryContent":
					// Send to inventory
					dataManager.Inventory.addItem (itemData);
					dataManager.Inventory.equipItem (new EmptyItemData (), previous_parent.name, previous_index);

					Destroy (gameObject);

					inventoryCanvas.updateInventory ();
					break;
				// Dropped onto EquipLeft or EquipRight
				case "EquipLeft":
				case "EquipRight":

					// Item can be placed in equip slot
					if (droppable (itemData, new_index)) {

						// Swap items
						dataManager.Inventory.equipItem (old_item.GetComponent<Item>().ItemData, previous_parent.name, previous_index);
						dataManager.Inventory.equipItem (gameObject.GetComponent<Item>().ItemData, new_parent.name, new_index);

						Destroy (gameObject);

						inventoryCanvas.updateInventory ();
					} else {

						// Send to inventory
						dataManager.Inventory.addItem (itemData);
						dataManager.Inventory.equipItem (new EmptyItemData (), previous_parent.name, previous_index);

						Destroy (gameObject);

						inventoryCanvas.updateInventory ();
					}
					break;
				// Dropped back onto ToolBar
				case "ToolBar":
					
					// Item is placed in original location
					if (previous_index == new_index) {

						// Return to original position
						returnToOriginalPosition ();

					} else {

						// Swap items
						dataManager.Inventory.equipItem (old_item.GetComponent<Item>().ItemData, previous_parent.name, previous_index);
						dataManager.Inventory.equipItem (gameObject.GetComponent<Item>().ItemData, new_parent.name, new_index);

						Destroy (gameObject);

						inventoryCanvas.updateInventory ();

					}
					break;
				}

				break;
			}

		} else {
			// Dropped off of item slot, return to inventory

			switch (previous_parent.name) {
			case "Inventory":
				// Return to original position
				returnToOriginalPosition ();
				break;
			default:
				// Send to inventory
				dataManager.Inventory.addItem (itemData);
				dataManager.Inventory.equipItem (new EmptyItemData (), previous_parent.name, previous_index);

				Destroy (gameObject);

				inventoryCanvas.updateInventory ();
				break;


			}

		}



	}
		
	private void returnToOriginalPosition() {
		Destroy (previous_parent.transform.GetChild (previous_index).gameObject);
		transform.SetParent (previous_parent.transform);
		transform.SetSiblingIndex (previous_index);
		gameObject.layer = LayerMask.NameToLayer ("Items");
	}

	private bool droppable(ItemData item, int index) {

		switch (index) {
		case 0:
			return (item is HighArmorData);
		case 1:
			return (item is MidArmorData);
		case 2:
			return (item is LowArmorData);
		default:
			return false;
		}


	}

	public void OnBeginDrag(PointerEventData eventData) {
		
		// Checking if left button click
		if (eventData.button != PointerEventData.InputButton.Left) {
			return;
		}

		// Storing previous location data
		previous_parent = transform.parent.gameObject;
		previous_index = transform.GetSiblingIndex ();

		// Creating empty item to hold position
		GameObject empty = Instantiate (canvas.GetComponent<InventoryCanvas> ().itemEmpty, transform.parent);
		empty.transform.SetSiblingIndex (transform.GetSiblingIndex ());
		empty.layer = LayerMask.NameToLayer ("Items");

		// Setting parent and layer data
		transform.SetParent (canvas);
		gameObject.layer = LayerMask.NameToLayer ("Held Items");

	}

	public void OnDrag(PointerEventData eventData) {
		
		// Checking if left button click
		if (eventData.button != PointerEventData.InputButton.Left) {
			return;
		}

		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.position = new Vector3(mousePos.x, mousePos.y, 0);
	}

	public void OnEndDrag(PointerEventData eventData) {
		
		// Checking if left button click
		if (eventData.button != PointerEventData.InputButton.Left) {
			return;
		}
			
		gameObject.GetComponent<Item> ().drop ();

		transform.localScale = new Vector3 (1, 1, 1);

		previous_parent = null;
		previous_index = 0;
	}

	public void OnPointerClick(PointerEventData eventData) {

		if (eventData.button != PointerEventData.InputButton.Right) {
			return;
		}

		canvas.GetComponent<InventoryCanvas> ().itemSelect.Item = gameObject;

	}
}
