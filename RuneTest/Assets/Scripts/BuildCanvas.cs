using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class BuildCanvas : MonoBehaviour {

	protected RectTransform page;
	protected RectTransform table;

	// Sprite to be placed behind rune slots that can be used
	public GameObject runeBack;

	public GameObject runeEmpty;
	public GameObject runeVoid;
	public GameObject runeBlock;

	// Dictionary to instantiate the correct runes from the pageData
	protected Dictionary<string,GameObject> runes;

	// DataManager
	public DataManager dataManager;

	// List to store filtered and available runes
	protected List<RuneData> tableRunes;
	protected string classFilter;
	protected string sortFilter;

	protected int numTablePages;
	protected int curPage;
	protected Bounds tableBounds;

	// Use this for initialization
	void Start () {

		initRunes ();
		initBuild ();

	}

	// Update is called once per frame
	void Update () {
		/*
		if (Input.GetMouseButtonDown (0)) {
			Debug.Log ("Mouse Down" + Input.mousePosition);
		}
		if (Input.GetMouseButtonUp (0)) {
			Debug.Log ("Mouse Up");
		}
		if (Input.GetMouseButton (0)) {
			//Debug.Log ("Mouse Press");
		}*/
		if (Input.GetAxis ("Mouse ScrollWheel") != 0f) {
			if (tableBounds.Contains(Input.mousePosition)) {
				if (Input.GetAxis ("Mouse ScrollWheel") > 0f) {
					//tableUp ();
				} else if (Input.GetAxis("Mouse ScrollWheel") < 0f) {
					//tableDown ();
				}
			}
		}
	}

	// Initializing the rune dictionary
	protected virtual void initRunes() {}

	// Initializing the UI elements
	protected virtual void initBuild() {}

	public void removeRune(int rune_idx, Vector3 pos) {

		// Creating an instance of an empty Rune
		GameObject instance = Instantiate(runeEmpty, pos, Quaternion.identity) as GameObject;
		instance.transform.localScale = page.localScale;
		instance.transform.SetParent (page,true);
		instance.transform.SetSiblingIndex (rune_idx + 1);

		// Destroying old Rune
		GameObject.Destroy(page.GetChild(rune_idx).gameObject);

	}

	public void replaceRune(int rune_idx, Vector3 pos, GameObject newRune) {

		// Destroying old Rune
		GameObject.Destroy(page.GetChild(rune_idx).gameObject);

		// Moving new Rune to page
		newRune.transform.SetParent (page);
		newRune.transform.position = pos;
		newRune.transform.SetSiblingIndex (rune_idx);

	}

	public void changeTable(string filterName) {
		/*
		//Debug.Log ("Changing Table");

		// Getting filter parameters
		string[] filter = filterName.Split ('_');

		// If the class Filter was changed
		if (filter [0] == "Class") {
			classFilter = filter [1];
			// If the rank Filter was changed
		} else {
			rankFilter = filter [1];
		}

		tableRunes = dataManager.getBuildData().getTable (classFilter,rankFilter);
		numTablePages = (int)Mathf.Ceil (tableRunes.Count / 6f);
		curPage = 0;
		updateTable ();*/

	}

	public void changeTable() {
		/*
		//Debug.Log ("Changing Table");

		tableRunes = dataManager.getBuildData().getTable (classFilter,rankFilter);
		numTablePages = (int)Mathf.Ceil (tableRunes.Count / 6f);
		curPage = 0;
		updateTable ();*/
	}

	public void updateTable() {

		foreach (Transform child in table) {
			GameObject.Destroy(child.gameObject);
		}
		foreach (RuneData rune in tableRunes) {
			GameObject instance = Instantiate (runes [rune.Id],new Vector3 (0,0,1), Quaternion.identity,table);
		}

		/*
		for (int i = 0;i < end_index-start_index;i++) {
			GameObject instance = Instantiate (runes [tableRunes], table);
			//GameObject instance = Instantiate(runes[tableRunes.Keys[i+start_index]], new Vector3 ((i%2)*1.5f, -(i-(i%2)),0F), Quaternion.identity) as GameObject;
			//instance.transform.SetParent (table);
			//instance.transform.localPosition = new Vector3 ((i % 2) * 1.5f, -(i - (i % 2)), 0F);
		}*/
	}

	public void tableUp() {
		/*
		//Debug.Log ("Table Up");
		if (curPage == 0) {
			return;
		} else {
			curPage--;
			updateTable ();
		}*/
	}

	public void tableDown() {

		/*
		//Debug.Log ("Table Down");
		if (curPage == numTablePages - 1) {
			return;
		} else {
			curPage++;
			updateTable ();
		}*/
	}

	public virtual bool pageCheck() {
		return true;
	}

}
