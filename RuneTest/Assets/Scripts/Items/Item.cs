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
	private Transform inventory;

	// Bounds of item slots
	private Bounds equipLeftBounds;
	private Bounds equipRightBounds;
	private Bounds toolBarBounds;
	private Bounds trash;			// Idk if will be used here

	// Dragging variables
	private bool drag;
	private Vector3 drag_start_position;
	private Vector3 mouse_offset;

	private GameObject previous_parent;
	private int previous_index;

	public ItemData ItemData { get { return itemData; } set { itemData = value; } }

	public bool Selected { get { return selected; } 
		set { selected = value;
			if (value) {
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
		inventory = GameObject.Find ("Inventory").transform;

		equipLeftBounds = new Bounds (equipLeft.localPosition, equipLeft.rect.size);
		equipRightBounds = new Bounds (equipRight.localPosition, equipRight.rect.size);
		toolBarBounds = new Bounds (toolBar.localPosition, toolBar.rect.size);

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
						if (!(old_item.GetComponent<Item> ().ItemData is EmptyItemData)) {
							dataManager.Inventory.addItem (old_item.GetComponent<Item>().ItemData);
							canvas.GetComponent<InventoryCanvas> ().updateInventory ();
						}
						// Move item to new parent
						dataManager.Inventory.equipItem (itemData, new_parent.name, new_index);
						dataManager.Inventory.removeItem (itemData);

						transform.SetParent (new_parent.transform);
						transform.SetSiblingIndex (new_index);
						gameObject.layer = LayerMask.NameToLayer ("Items");

						Destroy (new_parent.transform.GetChild (transform.GetSiblingIndex () + 1).gameObject);

					} else {
						returnToOriginalPosition ();
					}
					break;
					// Dropped onto ToolBar, check if can drop
				case "ToolBar":

					// Moving old item to inventory if exists
					if (!(old_item.GetComponent<Item> ().ItemData is EmptyItemData)) {
						dataManager.Inventory.addItem (old_item.GetComponent<Item>().ItemData);
						canvas.GetComponent<InventoryCanvas> ().updateInventory ();
					}

					// Move item to new parent
					dataManager.Inventory.equipItem (itemData, new_parent.name, new_index);
					dataManager.Inventory.removeItem (itemData);

					transform.SetParent (new_parent.transform);
					transform.SetSiblingIndex (new_index);
					gameObject.layer = LayerMask.NameToLayer ("Items");

					Destroy (new_parent.transform.GetChild (transform.GetSiblingIndex () + 1).gameObject);

					Destroy (previous_parent.transform.GetChild (previous_index).gameObject);

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
					canvas.GetComponent<InventoryCanvas> ().updateInventory ();

					Destroy (gameObject);
					break;
				// Dropped back onto EquipLeft, return to original position
				case "EquipLeft":
					returnToOriginalPosition ();
					break;
				// Dropped onto EquipRight, check if valid move
				case "EquipRight":
					// If the item types are the same
					if (previous_index == new_index) {
						// Swap two equips

					} else {
						// Item types different, send to inventory
						dataManager.Inventory.addItem (itemData);
						canvas.GetComponent<InventoryCanvas> ().updateInventory ();

						Destroy (gameObject);
					}
					//*** Check if equip is move to valid position
					//if valid, swap two equips
					//if not, send to inventory
					break;
				// Dropped onto ToolBar, move to toolbar, and check if item can be swapped
				case "ToolBar":
					//*** Check if old item can be moved to old equip position
					//if yes, swap positions
					//if no, send old item to inventory
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
					canvas.GetComponent<InventoryCanvas> ().updateInventory ();

					Destroy (gameObject);
					break;
					// Dropped onto EquipLeft, check if valid move
				case "EquipLeft":
					//*** check if equip is move to valid position
					//if valid swap two equips
					//if not, send ot iventory
					break;
					// Dropped back onto EquipRight, return to original position
				case "EquipRight":
					returnToOriginalPosition ();
					break;
					// Dropped onto ToolBar, move to toolbar, and check if item can be swapped
				case "ToolBar":
					//*** Check if old item can be moved to old equip position
					//if yes, swap positions
					//if no, send old item to inventory
					break;
				}

				break;
			// Came from ToolBar
			case "ToolBar":

				switch (new_parent.name) {
				// Dropped into Inventory
				case "InventoryContent":
					//*** Send ot inventory
					break;
				// Dropped onto EquipLeft
				case "EquipLeft":
					//*** Check if can move to equip slot
					//if yes, swap items
					//if no, send ot inventory
					break;
					// Dropped onto EquipRight
				case "EquipRight":
					//*** Check if can move to equip slot
					//if yes, swap items
					//if no, send ot inventory
					break;
				// Dropped back onto ToolBar
				case "Toolbar":
					//*** check same location
					//if yes, return to old position
					//if no, swap items
					break;
				}

				break;
			}

		} else {
			// Dropped off of item slot, return to original position
			returnToOriginalPosition ();
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
			return true;
		case 1:
			return true;
		case 2:
			return true;
		default:
			return true;
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

		// Initializing dragging
		drag_start_position = transform.position;
		mouse_offset = transform.position - new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0);
		mouse_offset = Vector3.zero;
		this.drag = true;

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
		transform.position = new Vector3(mousePos.x, mousePos.y, 0) + mouse_offset;
	}

	public void OnEndDrag(PointerEventData eventData) {
		
		// Checking if left button click
		if (eventData.button != PointerEventData.InputButton.Left) {
			return;
		}

		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0) + mouse_offset;
	
		mouse_offset = Vector3.zero;
		this.drag = false;
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
