using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildCanvas : MonoBehaviour {

	// Rune number
	protected int sides;

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

	// Data Variables
	protected TableData tableRunes;
	protected string classFilter;

	protected PageData pageRunes;

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
		tableRunes.Table.Add (rune);
	}

	public void removeFromTable(RuneData rune) {
		tableRunes.Table.Remove (rune);
	}

	public void addToPage(RuneData rune, int index, int rotation) {

		int page_w = pageRunes.Page.GetLength (1);

		int w = index % page_w;
		int h = (index - w) / page_w;

		pageRunes.Page [h, w] = rune;
		pageRunes.PageRotations [h, w] = rotation;

	}

	public void removeFromPage(int index) {

		int page_w = pageRunes.Page.GetLength (1);

		int w = index % page_w;
		int h = (index - w) / page_w;

		pageRunes.Page [h, w] = new EmptyData();
		pageRunes.PageRotations [h, w] = 0;

	}

	public void changeTable(string filterName) {

		classFilter = filterName;

		updateRunes ();

	}

	public void updateRunes() {

		// Removing old table runes
		foreach (Transform child in table) {
			Destroy(child.gameObject);
		}
		table.DetachChildren ();
		// Removing table rune backs
		foreach (Transform child in tableBack) {
			Destroy (child.gameObject);
		}
		tableBack.DetachChildren ();
		// Removing old page runes
		foreach (Transform child in page) {
			Destroy (child.gameObject);
		}
		page.DetachChildren ();
		foreach (Transform child in pageBack) {
			Destroy (child.gameObject);
		}
		pageBack.DetachChildren ();

		// Getting runes based on filter
		List<RuneData> filteredRunes = tableRunes.getTable (classFilter);

		// Instantiating all filtered runes
		foreach (RuneData rune in filteredRunes) {
			GameObject instance = Instantiate (runes [rune.Id],new Vector3 (0,0,1), Quaternion.identity,table);
			instance.GetComponent<Rune> ().RuneData = rune;
			instance.GetComponent<Rune> ().SignalReceiver = signalReceiver;
			instance.layer = LayerMask.NameToLayer ("Table Runes");
			Instantiate (runeBack, new Vector3 (0, 0, 1), Quaternion.identity, tableBack);
		}

		// Updating size of TableContent and TableBack
		RectTransform content = (RectTransform)table.parent.transform;
		content.sizeDelta = new Vector2 (content.rect.size.x, ((tableRunes.getTable().Count + 3) / 4) * table.localScale.x);

		int page_h = pageRunes.Page.GetLength (0);
		int page_w = pageRunes.Page.GetLength (1);
		RuneData[,] pageData = pageRunes.Page;
		int[,] pageRotationData = pageRunes.PageRotations;

		// Instantiating all page runes
		for (int i = 0; i < page_h; i++) {
			for (int j = 0; j < page_w; j++) {
				GameObject instance = Instantiate (runes[pageData[i,j].Id], new Vector3 (i, j, 0F), Quaternion.identity, page) as GameObject;
				instance.GetComponent<Rune> ().RuneData = pageData [i, j];
				instance.GetComponent<Rune> ().SignalReceiver = signalReceiver;
				instance.GetComponent<Rune> ().Rotation = pageRotationData [i, j];
				instance.transform.Rotate (Vector3.forward * pageRotationData [i, j] * 360 / sides);
				instance.layer = LayerMask.NameToLayer ("Page Runes");

				if (pageData[i,j].Id == "Void") {
					Instantiate (runeVoid, new Vector3 (i, j, 0F), Quaternion.identity, pageBack);
				} else {
					Instantiate (runeBack, new Vector3 (i, j, 0F), Quaternion.identity, pageBack);
				}
			}
		}

		bool found = false;

		// Finding if selected rune still exists

		//Debug.Log ("Searching for selected rune");

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

				//Debug.Log ("Searching for selected rune in page");

				// Searching in page for selected rune
				foreach (Transform child in page) {

					Debug.Log (child);

					if (child.gameObject.GetComponent<Rune> ().RuneData == selectedRuneData) {
						//Debug.Log ("Found");

						runeSelect.GetComponent<RuneSelect> ().Rune = child.gameObject;
						child.gameObject.GetComponent<Rune> ().Selected = true;
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

	public void exitScene() {
		SceneManager.LoadScene (dataManager.LastScene);
	}

}
