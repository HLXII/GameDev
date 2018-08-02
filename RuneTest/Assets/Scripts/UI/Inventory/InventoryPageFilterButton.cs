using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPageFilterButton : MonoBehaviour {

	private bool state;

	private InventoryCanvas inventoryCanvas;
	public GameObject inventoryFilter;

	// Use this for initialization
	void Start () {
		inventoryCanvas = GameObject.Find ("Canvas").GetComponent<InventoryCanvas> ();

		this.State = (gameObject.name == "Consumable");

		gameObject.GetComponent<Button> ().onClick.AddListener (this.onClick);
	}

	public bool State
	{
		get
		{
			return state;
		}
		set
		{
			if (value) {
				gameObject.GetComponent<Image> ().color = new Color (0, 0, 0);
				inventoryFilter.transform.SetAsLastSibling ();
			} else {
				gameObject.GetComponent<Image> ().color = new Color (255, 255, 255);
			}
			state = value;
		}
	}

	// Update is called once per frame
	void Update () {

	}

	public void onClick() {
		for (int i = 0; i < transform.parent.childCount; i++) {
			transform.parent.GetChild (i).GetComponent<InventoryPageFilterButton> ().State = false;
		}
		this.State = true;
	}
}
