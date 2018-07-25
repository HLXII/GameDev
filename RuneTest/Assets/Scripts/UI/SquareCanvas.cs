﻿using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

public class SquareCanvas : BuildCanvas {

	// All possible runes to be placed
	public GameObject runeSingleWire;
	public GameObject runeCorner;
	public GameObject runeCross;
	public GameObject runeSource;
	public GameObject runeSink;


	// List to store filtered and available runes
	//private SortedList<string,int> tableRunes;
	//private string classFilter;
	//private string rankFilter;
	//private int numTablePages;
	//private int curPage;
	//private Bounds tableBounds;

	// Use this for initialization
	void Start () {

		signalReceiver = GameObject.Find ("BuildSignals").GetComponent<BuildSignalText> ();

		initRunes ();
		initBuild ();

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown ("left shift")) {
			Debug.Log ("SHIFT DOWN");
			for (int i = 0; i < page.childCount; i++) {
				page.GetChild (i).GetComponent<Rune> ().Active = false;
			}
		}

		if (Input.GetKeyUp ("left shift")) {
			for (int i = 0; i < page.childCount; i++) {
				page.GetChild (i).GetComponent<Rune> ().Active = true;
			}
		}
			
	}

	// Initializing the rune dictionary
	protected override void initRunes() {
		this.runes = new Dictionary<string,GameObject> () {
			{ "Empty",runeEmpty },
			{ "Void",runeVoid },
			{ "Block",runeBlock },

			{ "SingleWire",runeSingleWire },
			{ "Corner",runeCorner },
			{ "Cross",runeCross },

			{ "Source", runeSource },
			{ "Sink", runeSink }
		};
	}

	// Initializing the UI elements
	protected override void initBuild() {
		
		//Debug.Log ("Initializing Build");

		// Getting BuildSignalManager
		signalReceiver = GameObject.Find("BuildSignals").GetComponent<BuildSignalText>();

		// Getting buildData from DataManager
		dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();
		BuildData buildData = GameObject.Find ("DataManager").GetComponent<DataManager> ().getBuildData ();

		// Getting data from buildData
		tableRunes = buildData.getTable ();
		RuneData[,] pageData = buildData.getPage ();
		int[,] pageRotationData = buildData.getPageRotation ();

		// Creating the Table object to hold all the runes in the table
		table = (RectTransform) GameObject.Find("Table").transform.GetChild(0).GetChild(0).GetChild(0);

		// Setting scale of TableContent to fit table window
		Rect tableRect = ((RectTransform)table.parent.parent.parent.transform).rect;
		table.localScale = new Vector3 (tableRect.size.x / 200f, tableRect.size.x / 200f, 1);

		// Calculating the table parameters and updating the table
		//rankFilter = "";
		classFilter = "";
		numTablePages = (int)Mathf.Ceil (tableRunes.Count / 6f);
		curPage = 0;
		updateTable ();

		// Creating the Page object to hold all the runes in the page
		page = (RectTransform) GameObject.Find("Page").transform.GetChild(0).GetChild(0).transform;

		int page_h = pageData.GetLength (0);
		int page_w = pageData.GetLength (1);
		page.sizeDelta = new Vector2 (page_w * 100, page_h * 100);
		page.localScale = new Vector3 (Screen.width / 1600f, Screen.width / 1600f, 1);

		// Instantiating all the runes in the page from the pageData
		for (int i = 0; i < page_h; i++) {
			for (int j = 0; j < page_w; j++) {
				GameObject instance = Instantiate (runes[pageData[i,j].Id], new Vector3 (i, j, 0F), Quaternion.identity, page) as GameObject;
				instance.GetComponent<Rune> ().RuneData = pageData [i, j];
				instance.GetComponent<Rune> ().SignalReceiver = signalReceiver;
				instance.GetComponent<Rune> ().Rotation = pageRotationData [i, j];
				// sides for the Rune object haven't been initialized, thus have to hard code it
				// Be careful for other build canvases to change this
				instance.transform.Rotate (Vector3.forward * pageRotationData [i, j] * 90);
				instance.layer = 9;
			}
		}
	}

	public override bool pageCheck() {
		for (int i = 0; i < page.childCount; i++) {
			page.GetChild (i).GetComponent<Rune> ().findNeighbors ();
			//page.GetChild (i).GetChild (0).GetComponent<Animator> ().SetTrigger ("right_to_left");
		}
		for (int i = 0; i < page.childCount; i++) {
			if (!page.GetChild (i).GetComponent<Rune> ().checkNeighbors ()) {
				return false;
			}
		}
		return true;
	}

	public void btnCheck() {
		Debug.Log (pageCheck ());
	}

	public void simulate() {
		Debug.Log ("Resetting Runes");
		for (int i = 0; i < page.childCount; i++) {
			page.GetChild (i).GetComponent<Rune> ().reset ();
		}

		Debug.Log ("Finding Neighbors");
		for (int i = 0; i < page.childCount; i++) {
			page.GetChild (i).GetComponent<Rune> ().findNeighbors ();
		}

		Debug.Log ("Resetting SignalReceiver");
		signalReceiver.reset ();

		Debug.Log ("Starting Simulation");
		InvokeRepeating ("simulationStep", 0f, .2f);
		//CancelInvoke();

	}

	public void endSimulate() {
		CancelInvoke ();
	}

	public void simulationStep() {

		//Debug.Log ("Simulation Step");
		for (int i = 0; i < page.childCount; i++) {
			page.GetChild (i).GetComponent<Rune> ().sendEnergy ();
		}
		for (int i = 0; i < page.childCount; i++) {
			page.GetChild (i).GetComponent<Rune> ().manipulateEnergy ();
		}
		//Debug.Log (page.GetChild (0).GetChild (0).GetComponent<Animator> ().runtimeAnimatorController.animationClips[0].length);
	}

	/*
	public void changeTable(string filterName) {

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
		updateTable ();

	}

	public void updateTable() {
		foreach (Transform child in table) {
			GameObject.Destroy(child.gameObject);
		}
		int start_index = curPage * 6;
		int end_index = Mathf.Min ((curPage + 1) * 6, tableRunes.Count);
		for (int i = 0;i < end_index-start_index;i++) {
			//Debug.Log ((i%2)*1.5f);
			GameObject instance = Instantiate(runes[tableRunes.Keys[i+start_index]], new Vector3 ((i%2)*1.5f, -(i-(i%2)),0F), Quaternion.identity) as GameObject;
			instance.transform.SetParent (table);
			instance.transform.localPosition = new Vector3 ((i % 2) * 1.5f, -(i - (i % 2)), 0F);
		}
	}

	public void tableUp() {
		//Debug.Log ("Table Up");
		if (curPage == 0) {
			return;
		} else {
			curPage--;
			updateTable ();
		}
	}

	public void tableDown() {
		//Debug.Log ("Table Down");
		if (curPage == numTablePages - 1) {
			return;
		} else {
			curPage++;
			updateTable ();
		}
	}*/

}