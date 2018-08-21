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

	// Reference to the Rune Select
	public GameObject runeSelect;

	// Reference to BuildSignalManager
	protected BuildSignalText signalReceiver;

	// Rune Base sprites by rank
	public Sprite[] runeBases;
    // Rune Select Outline
    public GameObject runeSelectOutline;
    // Rune
    public GameObject rune;

    // Shared runeIds
    public string emptyId;
    public string voidId;

	// DataManager
	public DataManager dataManager;

	// Data Variables
	protected TableData tableData;
	protected string classFilter;
	protected PageData pageData;

    // Simulation
    protected bool simulating;

	// Use this for initialization
	void Start () {
		
		InitRunes ();
		InitBuild ();

	}

	// Update is called once per frame
	void Update () {

        if (simulating)
        {

        }

	}

	// Initializing the rune dictionary
	protected virtual void InitRunes() {}

	// Initializing the UI elements
	protected virtual void InitBuild() {}

	public void AddToTable(RuneSlot runeSlot) {
        tableData.Table.Add (runeSlot.RuneData);
	}

	public void RemoveFromTable(RuneSlot runeSlot) {
        tableData.Table.Remove (runeSlot.RuneData);
	}

	public void AddToPage(RuneSlot runeSlot, int index) {

		int page_w = pageData.Page.GetLength (1);

		int w = index % page_w;
		int h = (index - w) / page_w;

        pageData.Page[h, w] = runeSlot;

	}

	public void RemoveFromPage(int index) {

		int page_w = pageData.Page.GetLength (1);

		int w = index % page_w;
		int h = (index - w) / page_w;

        pageData.Page[h, w] = new RuneSlot(new RuneData(emptyId));

	}

	public void ChangeTable(string filterName) {

		classFilter = filterName;

        UpdateTable();

	}

    public void UpdatePage()
    {
        //Debug.Log("Updating Page");

        // Removing old page runes
        foreach (Transform child in page)
        {
            Destroy(child.gameObject);
        }
        page.DetachChildren();

        int page_h = pageData.Page.GetLength(0);
        int page_w = pageData.Page.GetLength(1);
        RuneSlot[,] runeSlots = pageData.Page;

        // Instantiating all page runes
        for (int i = 0; i < page_h; i++)
        {
            for (int j = 0; j < page_w; j++)
            {
                GameObject instance = Instantiate(rune, new Vector3(i, j, 0F), Quaternion.identity, page) as GameObject;
                instance.GetComponent<Rune>().RuneSlot = runeSlots[i, j];
                instance.GetComponent<Rune>().SignalReceiver = signalReceiver;
                instance.layer = LayerMask.NameToLayer("Page Runes");
            }
        }

    }

    public void UpdateTable()
    {
        //Debug.Log("Updating Table");

        // Removing old table runes
        foreach (Transform child in table)
        {
            Destroy(child.gameObject);
        }
        table.DetachChildren();

        // Getting runes based on filter
        List<RuneData> filteredRunes = tableData.getTable(classFilter);

        // Instantiating all filtered runes
        foreach (RuneData runeData in filteredRunes)
        {
            GameObject instance = Instantiate(rune, new Vector3(0, 0, 1), Quaternion.identity, table);
            instance.GetComponent<Rune>().RuneSlot = new RuneSlot(runeData);
            instance.GetComponent<Rune>().SignalReceiver = signalReceiver;
            instance.layer = LayerMask.NameToLayer("Table Runes");
        }

        // Updating size of TableContent and TableBack
        RectTransform content = (RectTransform)table.parent.transform;
        content.sizeDelta = new Vector2(content.rect.size.x, ((tableData.getTable().Count + 3) / 4) * table.localScale.x);

    }

	public void UpdateSelection() {

		bool found = false;

		// Finding if selected rune still exists

		//Debug.Log ("Searching for selected rune");

		RuneData selectedRuneData = runeSelect.GetComponent<RuneSelect>().RuneData;

		// If no rune selected, ignore
		if (selectedRuneData != null) {

			// Searching through table
			foreach (Transform child in table) {

				// If the selected rune exists in the table, set it as selected and end
				if (child.gameObject.GetComponent<Rune>().RuneSlot.RuneData == selectedRuneData) {
					//Debug.Log ("Found");
                    child.gameObject.GetComponent<Rune>().OnSelect();
					found = true;
					break;
				}
			}

			// If rune hasn't been found, it may be in the page
			if (!found) {

				//Debug.Log ("Searching for selected rune in page");

				// Searching in page for selected rune
				foreach (Transform child in page) {

					if (child.gameObject.GetComponent<Rune> ().RuneSlot.RuneData == selectedRuneData) {
						//Debug.Log ("Found");
                        child.gameObject.GetComponent<Rune>().OnSelect();
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

    public void EndSimulate()
    {
        simulating = false;
        CancelInvoke();
    }

    public void CleanSimulation()
    {
        pageData.Reset();
    }

    public void StartSimulation()
    {
        Debug.Log("Finding Neighbors");
        pageData.FindNeighbors();

        Debug.Log("Resetting SignalReceiver");
        signalReceiver.reset();

        pageData.Active = true;

        Debug.Log("Starting Simulation");
        InvokeRepeating("SimulationStep", 0f, .2f);
        //CancelInvoke();
    }

    public void SimulationStep()
    {
        pageData.SimulationStep();
    }


    public void ExitScene() {
		SceneManager.LoadScene (dataManager.LastScene);
	}

}
