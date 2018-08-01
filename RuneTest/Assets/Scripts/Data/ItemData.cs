using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData {

	protected string id;
	protected string description;

	public string Id { get { return id; } }
	public string Description { get { return description; } }

}

[System.Serializable]
public class ConsumableData : ItemData {

}

[System.Serializable]
public class WeaponData : ItemData {

}

[System.Serializable]
public class ArmorData : ItemData {

}

[System.Serializable]
public class RuneWeaponData : WeaponData {

	private PageData page;

	public PageData Page { get { return page; } }

}

[System.Serializable]
public class RuneArmorData : ArmorData {

	private PageData page;

	public PageData Page { get { return page; } }

}
