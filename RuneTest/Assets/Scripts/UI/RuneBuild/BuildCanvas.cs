using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class BuildCanvas : MonoBehaviour {

	// References to the page and table transforms
	protected RectTransform page;
	protected RectTransform table;
	// References to the backs of the table and page
	protected RectTransform pageBack;
	protected RectTransform tableBack;

	// Reference to the Rune Select
	public GameObject runeSelect;

	// Reference to BuildSignalManager
	protected BuildSignalText signalReceiver;

	// Rune Back
	public GameObject runeBack;
	// Rune Select Outline
	public GameObject runeSelectOutline;

	// Generic runes required by all rune pages
	public GameObject runeEmpty;
	public GameObject runeVoid;
	public GameObject runeBlock;

	// Dictionary to instantiate the correct runes from the pageData
	protected Dictionary<string,GameObject> runes;

	// DataManager
	public DataManager dataManager;

	// Table Rune variables
	protected TableData tableRunes;
	protected string classFilter;

	// Use this for initialization
	void Start () {

		initRunes ();
		initBuild ();

	}

	// Update is called once per frame
	void Update () {

	}

	// Initializing the rune dictionary
	protected virtual void initRunes() {}

	// Initializing the UI elements
	protected virtual void initBuild() {}

	public void addToTable(RuneData rune) {
		tableRunes.addToTable (rune);
		updateTable ();
	}

	public void removeFromTable(RuneData rune) {
		tableRunes.removeFromTable (rune);
	}

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

		classFilter = filterName;

		updateTable ();

	}

	public void updateTable() {

		// Removing old runes
		foreach (Transform child in table) {
			GameObject.Destroy(child.gameObject);
		}
		table.DetachChildren ();
		// Removing rune backs
		foreach (Transform child in tableBack) {
			GameObject.Destroy (child.gameObject);
		}
		tableBack.DetachChildren ();
		// Getting runes based on filter
		List<RuneData> filteredRunes = tableRunes.getTable (classFilter);

		// Instantiating all filtered runes
		foreach (RuneData rune in filteredRunes) {
			GameObject instance = Instantiate (runes [rune.Id],new Vector3 (0,0,1), Quaternion.identity,table);
			instance.GetComponent<Rune> ().RuneData = rune;
			instance.GetComponent<Rune> ().SignalReceiver = signalReceiver;
			instance.layer = 8;
			Instantiate (runeBack, new Vector3 (0, 0, 1), Quaternion.identity, tableBack);
		}

		// Updating size of TableContent and TableBack
		RectTransform content = (RectTransform)table.parent.transform;
		content.sizeDelta = new Vector2 (content.rect.size.x, ((tableRunes.getTable().Count + 3) / 4) * 40 * table.localScale.x);

		bool found = false;

		// Finding if selected rune still exists

		Debug.Log ("Searching for selected rune");

		RuneData selectedRuneData = runeSelect.GetComponent<RuneSelect>().RuneData;

		// If no rune selected, ignore
		if (selectedRuneData != null) {

			// Searching through table
			foreach (Transform child in table) {

				// If the selected rune exists in the table, set it as selected and end
				if (child.gameObject.GetComponent<Rune>().RuneData == selectedRuneData) {
					//Debug.Log ("Found");

					runeSelect.GetComponent<RuneSelect> ().Rune = child.gameObject;
					child.gameObject.GetComponent<Rune> ().Selected = true;
					found = true;
					break;
				}
			}

			// If rune hasn't been found, it may be in the page
			if (!found) {

				Debug.Log ("Searching for selected rune in page");

				// Searching in page for selected rune
				foreach (Transform child in page) {

					Debug.Log (child);

					if (child.gameObject.GetComponent<Rune> ().RuneData == selectedRuneData) {

						Debug.Log ("Found");
						found = true;
						break;
					}
				}
			}

			// If rune wasn't found in the table or page, it was filtered out, thus unselect
			if (!found) {
				//Debug.Log ("Not Found, clearing Selection");
				runeSelect.GetComponent<RuneSelect> ().clearSelect ();
			}
		}

	}

	public virtual void simulate () {}
	public virtual void endSimulate () {}
	public virtual void cleanSimulation () {}

}
