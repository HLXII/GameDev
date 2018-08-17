using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory {

	private List<RuneData> runes;

	[SerializeField]
	private List<ItemData> items;

	private ItemData[] equipLeft;
	private ItemData[] equipRight;
	private ItemData[] toolBar;

	//private List<ItemData> items;

	public ItemData[] EquipLeft { get { return equipLeft; } }
	public ItemData[] EquipRight { get { return equipRight; } }
	public ItemData[] ToolBar { get { return toolBar; } }

	// Temporary for tests
	public Inventory() {

		runes = new List<RuneData> ();
		items = new List<ItemData> ();

		for (int i = 0; i < 200; i++) {
			items.Add (new ItemData ("Chili"));
		}

		items.Add (new ItemData ("Nature Ring"));
		items.Add (new ItemData ("Hoverboard"));

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
