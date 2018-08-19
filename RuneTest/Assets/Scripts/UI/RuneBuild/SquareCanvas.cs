using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

public class SquareCanvas : BuildCanvas {
    
	// List to store filtered and available runes
	//private SortedList<string,int> tableRunes;
	//private string classFilter;
	//private string rankFilter;
	//private int numTablePages;
	//private int curPage;
	//private Bounds tableBounds;

	// Use this for initialization
	void Start () {

		sides = 4;

		initRunes ();
		initBuild ();

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
	protected override void initRunes() {

	}

	// Initializing the UI elements
	protected override void initBuild() {
		
		//Debug.Log ("Initializing Build");

		// Getting BuildSignalManager
		signalReceiver = GameObject.Find("BuildSignals").GetComponent<BuildSignalText>();

		// Getting buildData from DataManager
		dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();

		// Getting data from buildData
		tableRunes = dataManager.TableData;
		pageRunes = dataManager.PageData;

		// Creating the Table object to hold all the runes in the table
		table = (RectTransform) GameObject.Find("TableContent").transform;
		tableBack = (RectTransform)GameObject.Find ("TableBack").transform;

		// Creating the Page object to hold all the runes in the page
		page = (RectTransform) GameObject.Find("PageContent").transform;
		pageBack = (RectTransform)GameObject.Find ("PageBack").transform;

		// Initializing table parameters
		classFilter = "";

		// Initializing page
		RuneData[,] pageData = pageRunes.Page;
		int[,] pageRotationData = pageRunes.PageRotations;

		int page_h = pageData.GetLength (0);
		int page_w = pageData.GetLength (1);

		((RectTransform)page.parent.transform).sizeDelta = new Vector2 (page_w, page_h);
		page.sizeDelta = new Vector2 (page_w, page_h);
		pageBack.sizeDelta = page.sizeDelta;

		pageBack.GetComponent<GridLayoutGroup> ().constraint = GridLayoutGroup.Constraint.FixedColumnCount;
		pageBack.GetComponent<GridLayoutGroup> ().constraintCount = page_w;

        // Instantiating runes
        updatePage();
        updateTable();

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

	public override void simulate() {
		/*
		Debug.Log ("Resetting Runes");
		for (int i = 0; i < page.childCount; i++) {
			page.GetChild (i).GetComponent<Rune> ().reset ();
		}*/

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

	public override void endSimulate() {
		CancelInvoke ();
	}

	public override void cleanSimulation() {
		//Debug.Log ("Resetting Runes");
		//for (int i = 0; i < page.childCount; i++) {
		//	page.GetChild (i).GetComponent<Rune> ().reset ();
		//}

		//Debug.Log ("Finding Neighbors");
		//for (int i = 0; i < page.childCount; i++) {
		//	page.GetChild (i).GetComponent<Rune> ().findNeighbors ();
		//}
			
	}

	public void simulationStep() {

		////Debug.Log ("Simulation Step");
		//for (int i = 0; i < page.childCount; i++) {
		//	page.GetChild (i).GetComponent<Rune> ().sendEnergy ();
		//}
		//for (int i = 0; i < page.childCount; i++) {
		//	page.GetChild (i).GetComponent<Rune> ().manipulateEnergy ();
		//}

	}

}
