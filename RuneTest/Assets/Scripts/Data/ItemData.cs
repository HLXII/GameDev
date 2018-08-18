using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData {

	[SerializeField]
	private string itemTemplate;
	public string ItemTemplate { get { return itemTemplate; } }

	private PageData page;
	public PageData Page { get { return page; } }

	public ItemData(string itemTemplate) {
		this.itemTemplate = itemTemplate;
		page = null;
	}

	public ItemData(string itemTemplate, PageData page) {
		this.itemTemplate = itemTemplate;
		this.page = page;
	}
}
