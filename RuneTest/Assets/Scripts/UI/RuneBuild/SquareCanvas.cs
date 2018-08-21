using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

public class SquareCanvas : BuildCanvas {

	// Use this for initialization
	void Start () {

		sides = 4;

		InitRunes ();
		InitBuild ();

	}
	
	// Update is called once per frame
	void Update () {

		// May be able to move this into the SquarePage script, depending on if key preferences are made later on
		if (Input.GetKeyDown ("left shift")) {
			page.GetComponent<SquarePage>().Active = false;
		}

		if (Input.GetKeyUp ("left shift")) {
			page.GetComponent<SquarePage>().Active = true;
		}


			
	}

	// Initializing the rune dictionary
	protected override void InitRunes() {

	}

	// Initializing the UI elements
	protected override void InitBuild() {
		
		//Debug.Log ("Initializing Build");

		// Getting BuildSignalManager
		signalReceiver = GameObject.Find("BuildSignals").GetComponent<BuildSignalText>();

		// Getting buildData from DataManager
		dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();

		// Getting data from buildData
		tableData = dataManager.TableData;
		pageData = dataManager.PageData;

		// Creating the Table object to hold all the runes in the table
		table = (RectTransform) GameObject.Find("TableContent").transform;

		// Creating the Page object to hold all the runes in the page
		page = (RectTransform) GameObject.Find("PageContent").transform;

		// Initializing table parameters
		classFilter = "";

        // Initializing page
        RuneSlot[,] runeSlots = pageData.Page;
        pageData.BuildSignalManager = signalReceiver;

		int page_h = runeSlots.GetLength (0);
		int page_w = runeSlots.GetLength (1);

		((RectTransform)page.parent.transform).sizeDelta = new Vector2 (page_w, page_h);
        page.sizeDelta = new Vector2(page_w, page_h);

        // Instantiating runes
        UpdatePage();
        UpdateTable();

	}

//	public override bool pageCheck() {
//		for (int i = 0; i < page.childCount; i++) {
//			page.GetChild (i).GetComponent<Rune> ().findNeighbors ();
//			//page.GetChild (i).GetChild (0).GetComponent<Animator> ().SetTrigger ("right_to_left");
//		}
//		for (int i = 0; i < page.childCount; i++) {
//			if (!page.GetChild (i).GetComponent<Rune> ().checkNeighbors ()) {
//				return false;
//			}
//		}
//		return true;
//	}

}
