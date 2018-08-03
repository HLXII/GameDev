using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory {

	private List<RuneData> runes;

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

		for (int i = 0; i < 10; i++) {
			items.Add (new ChiliItemData ());
		}

		items.Add (new NatureRingData ());
		items.Add (new NatureRingData ());
		items.Add (new PinkBraData ());
		items.Add (new SnakeBootData ());
		items.Add (new TrashCanData());
		items.Add (new HoverboardData ());
		items.Add (new TopHatData ());

		equipLeft = new ItemData[3];
		equipRight = new ItemData[3];
		toolBar = new ItemData[6];


		for (int i = 0; i < 3; i++) {
			equipLeft[i] = new EmptyItemData ();
			equipRight[i] = new EmptyItemData ();
		}

		for (int i = 0; i < 6; i++) {
			toolBar[i] = new EmptyItemData ();
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
		if (!(item is EmptyItemData)) {
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
		return (item is ConsumableData);
	}
	private static bool FindWeapon(ItemData item)
	{
		return (item is WeaponData);
	}
	private static bool FindArmor(ItemData item)
	{
		return (item is ArmorData);
	}
	private static bool FindKey(ItemData item)
	{
		return (item is KeyData);
	}

}
