using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item", order = 1)]
public class ItemTemplate : ScriptableObject {

	public Sprite icon;

	public string id;
	public string description;

	public bool isConsumable;

	public bool isWeapon;

	public bool isArmor;

	public enum ArmorSlot { High, Mid, Low };
	public ArmorSlot armorSlot;

	public bool isKey;

	public bool isRuneable;
}
