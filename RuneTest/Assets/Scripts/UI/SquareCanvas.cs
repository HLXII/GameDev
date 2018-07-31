using System.Collections;
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

		// Getting data from buildData
		tableRunes = dataManager.TableData;
		RuneData[,] pageData = dataManager.PageData.Page;
		int[,] pageRotationData = dataManager.PageData.PageRotations;

		// Creating the Table object to hold all the runes in the table
		table = (RectTransform) GameObject.Find("TableContent").transform;
		tableBack = (RectTransform)GameObject.Find ("TableBack").transform;

		// Setting scale of TableContent to fit table window
		Rect tableRect = ((RectTransform)table.parent.parent.parent.transform).rect;
		table.localScale = new Vector3 (tableRect.size.x / 200f, tableRect.size.x / 200f, 1);
		tableBack.localScale = table.localScale;

		// Calculating the table parameters and updating the table
		//rankFilter = "";
		classFilter = "";
		updateTable ();

		// Creating the Page object to hold all the runes in the page
		page = (RectTransform) GameObject.Find("PageContent").transform;
		pageBack = (RectTransform)GameObject.Find ("PageBack").transform;

		int page_h = pageData.GetLength (0);
		int page_w = pageData.GetLength (1);
		((RectTransform)page.parent.transform).sizeDelta = new Vector2 (page_w * 100, page_h * 100);
		page.sizeDelta = new Vector2 (page_w * 100, page_h * 100);
		pageBack.sizeDelta = page.sizeDelta;
		//page.localScale = new Vector3 (Screen.width / 1600f, Screen.width / 1600f, 1);
		//pageBack.localScale = page.localScale;

		pageBack.GetComponent<GridLayoutGroup> ().constraint = GridLayoutGroup.Constraint.FixedColumnCount;
		pageBack.GetComponent<GridLayoutGroup> ().constraintCount = page_w;

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

				if (pageData[i,j].Id == "Void") {
					Instantiate (runeVoid, new Vector3 (i, j, 0F), Quaternion.identity, pageBack);
				} else {
					Instantiate (runeBack, new Vector3 (i, j, 0F), Quaternion.identity, pageBack);
				}
			}
		}
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
		Debug.Log ("Resetting Runes");
		for (int i = 0; i < page.childCount; i++) {
			page.GetChild (i).GetComponent<Rune> ().reset ();
		}

		Debug.Log ("Finding Neighbors");
		for (int i = 0; i < page.childCount; i++) {
			page.GetChild (i).GetComponent<Rune> ().findNeighbors ();
		}
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

}
