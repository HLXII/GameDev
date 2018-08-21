using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory {

	private List<RuneData> runes;
	public List<RuneData> Runes { get { return runes; } }

	[SerializeField]
	private List<ItemData> items;

	private ItemData[] equipLeft;
	private ItemData[] equipRight;
	private ItemData[] toolBar;

	//private List<ItemData> items;

	public ItemData[] EquipLeft { get { return equipLeft; } }
	public ItemData[] EquipRight { get { return equipRight; } }
	public ItemData[] ToolBar { get { return toolBar; } }

    public Inventory() {
        //Debug.Log("Loading Empty Inventory");
    }

	// Temporary for tests
	public Inventory(int test) {

        //Debug.Log("Loading Test Inventory");

        runes = new List<RuneData>
        {
            new RuneData("Square Single Wire"),
            new RuneData("Square Single Wire", 2, 3, 0),
            new RuneData("Square Energy Node"),
            new RuneData("Square Energy Sink",3,1,1),
            new RuneData("Square Single Wire", 5,0,0),
            new RuneData("Square Single Wire", 0,5,0)
        };
        /*
		runes.Add (new SquareSingleWireData(0,20));
		runes.Add (new SquareSingleWireData (5,30));
		runes.Add (new SquareCrossData (3, 20));
		runes.Add (new SquareSourceData (20));
		runes.Add (new SquareSourceData (10));
		runes.Add (new SquareCornerData (10, 20));
		runes.Add (new SquareSinkData (10, 100, 5));
		runes.Add (new SquareSinkData (20, 100, 10));
        */
        items = new List<ItemData> ();

		for (int i = 0; i < 20; i++) {
			items.Add (new ItemData ("Chili"));
		}

		items.Add (new ItemData ("Nature Ring"));
		items.Add (new ItemData ("Hoverboard"));
		items.Add (new ItemData ("Stick", new PageData(1)));

		equipLeft = new ItemData[3];
		equipRight = new ItemData[3];
		toolBar = new ItemData[6];


		for (int i = 0; i < 3; i++) {
			equipLeft [i] = new ItemData ("Empty Item");
			equipRight [i] = new ItemData ("Empty Item");
		}

		for (int i = 0; i < 6; i++) {
			toolBar[i] = new ItemData ("Empty Item");
		}

	}
		
	public void equipItem(ItemData item, string location, int index) {
		switch (location) {
		case "EquipLeft":
			equipLeft [index] = item;
			break;
		case "EquipRight":
			equipRight [index] = item;
			break;
		case "ToolBar":
			toolBar [index] = item;
			break;
		}
	}

	public void addItem(ItemData item) {
		if (!(DataManager.item[item.ItemTemplate].id == "Empty Item")) {
			items.Add (item);
		}
	}

	public void removeItem(ItemData item) {
		items.Remove (item);
	}

	public List<ItemData> getItems(string typeFilter) {
		switch (typeFilter) {
		case "Consumable":
			return items.FindAll (FindConsumable);
		case "Weapon":
			return items.FindAll (FindWeapon);
		case "Armor":
			return items.FindAll (FindArmor);
		case "Key":
			return items.FindAll (FindKey);
		default:
			return items;
		}
	}

	private static bool FindConsumable(ItemData item)
	{
		return DataManager.item[item.ItemTemplate].isConsumable;
	}
	private static bool FindWeapon(ItemData item)
	{
		return DataManager.item[item.ItemTemplate].isWeapon;
	}
	private static bool FindArmor(ItemData item)
	{
		return DataManager.item[item.ItemTemplate].isArmor;
	}
	private static bool FindKey(ItemData item)
	{
		return DataManager.item[item.ItemTemplate].isKey;
	}

}
