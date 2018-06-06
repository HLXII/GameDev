using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class SquareCanvas : BuildCanvas {

	//private Transform page;
	//private Transform table;

	// Sprite to be placed behind rune slots that can be used
	//public GameObject runeBack;

	// All possible runes to be placed
	//public GameObject runeVoid;
	//public GameObject runeEmpty;
	//public GameObject runeBlock;
	public GameObject runeSource;
	public GameObject runeSink;
	public GameObject runeSingleWire;
	public GameObject runeTJunction;
	public GameObject runeFourWay;
	public GameObject runeCross;

	// Dictionary to instantiate the correct runes from the pageData
	//private Dictionary<string,GameObject> runes;

	// DataManager
	//private DataManager dataManager;

	// List to store filtered and available runes
	//private SortedList<string,int> tableRunes;
	//private string classFilter;
	//private string rankFilter;
	//private int numTablePages;
	//private int curPage;
	//private Bounds tableBounds;


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
					tableUp ();
				} else if (Input.GetAxis("Mouse ScrollWheel") < 0f) {
					tableDown ();
				}
			}
		}
	}

	// Initializing the rune dictionary
	protected override void initRunes() {
		this.runes = new Dictionary<string,GameObject> () {
			{"S_Special_Void_0",runeVoid},
			{"S_Special_Empty_0",runeEmpty},
			{"S_Special_Block_0",runeBlock},
			{"S_Input_Source_0",runeSource},
			{"S_Output_Sink_0",runeSink},
			{"S_Wire_Single_0",runeSingleWire},
			{"S_Wire_TJunction_0",runeTJunction},
			{"S_Wire_FourWay_0",runeFourWay},
			{"S_Wire_Cross_0",runeCross}
		};
	}

	// Initializing the UI elements
	protected override void initBuild() {
		
		//Debug.Log ("Initializing Build");

		// Getting buildData from DataManager
		dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();
		BuildData buildData = GameObject.Find ("DataManager").GetComponent<DataManager> ().getBuildData ();

		// Getting data from buildData
		tableRunes = buildData.getTable ("","");
		string[,] pageData = buildData.getPage ();

		/*
		// Debug prints for table and page
		for (int i = 0; i < tableData.Count; i++) {
			Debug.Log (tableData.Keys[i]);
		}

		for (int i = 0; i < pageData.GetLength (0); i++) {
			for (int j = 0; j < pageData.GetLength (1); j++) {
				Debug.Log (pageData [i, j]);
			}
		}*/

		// Creating the Table object to hold all the runes in the table
		//table = transform.GetChild(2).transform;
		table = new GameObject("Table").transform;
		table.SetParent (gameObject.transform);

		// Finding the bounds of the table for determining scroll events on the table
		tableBounds = new Bounds (new Vector3 (Screen.width * 2.25f / 16f, Screen.height * 3.75f / 9f), new Vector3 (Screen.width * 3.5f / 16f, Screen.height * 6.5f / 9f));
		//print ("TABLE BOUNDS: " + tableBounds);

		// Calculating the table parameters and updating the table
		rankFilter = "";
		classFilter = "";
		numTablePages = (int)Mathf.Ceil (tableRunes.Count / 6f);
		curPage = 0;
		updateTable ();

		/*
		foreach (KeyValuePair<string, int> item in tableData) {
			
			float x = .5f + k % 2;
			float y = .5f - (k - (k % 2)) / 2;
			GameObject instance = Instantiate (runes [item.Key], new Vector3 (x, y, 0F), Quaternion.identity) as GameObject;
			instance.transform.SetParent (table, true);
		}*/

		// Creating the Page object to hold all the runes in the page
		page = new GameObject ("Page").transform;
		page.SetParent (gameObject.transform);

		int page_h = pageData.GetLength (1);
		int page_w = pageData.GetLength (0);

		// Instantiating all the runes in the page from the pageData
		for (int i = 0; i < page_w; i++) {
			for (int j = 0; j < page_h; j++) {
				GameObject instance = Instantiate (runes[pageData[i,j]], new Vector3 (i+.5f, j+.5f, 0F), Quaternion.identity) as GameObject;
				instance.transform.SetParent (page,true);
			}
		}

		// Repositioning all objects on the screen correctly

		// Getting Camera
		Camera cam = GameObject.Find ("Main Camera").GetComponent<Camera> ();

		//Debug.Log ("Screen: W" + Screen.width + " H" + Screen.height);

		// Screen height and width in units
		float screen_h = (2 * cam.orthographicSize);
		float screen_w = (2 * cam.orthographicSize * cam.aspect);

		// Height and width of the page screen that the page can be displayed on in units
		float page_screen_w = screen_w * 10 / 16;
		float page_screen_h = screen_h / 2;

		//Debug.Log ("Page Screen: W" + page_screen_w + " H" + page_screen_h);

		float h_scale = page_screen_h / page_h;
		float w_scale = page_screen_w / page_w;

		//Debug.Log ("HSCALE: " + h_scale + " WSCALE: " + w_scale);

		// Bring page position to top left corner, then to where it should be
		this.page.localPosition -= new Vector3 (screen_w/2, screen_h/2, 0);
		this.page.localPosition += new Vector3 (screen_w*5/16, screen_h*3.5f/9, 0);

		float scale = Mathf.Min(2, (h_scale < w_scale) ? h_scale : w_scale);

		// Finding new sprite widths and heights with new scale
		float new_sprite_w = page_w * scale;
		float new_sprite_h = page_h * scale;

		this.page.localScale = new Vector3 (scale, scale, 1);
		this.page.localPosition += new Vector3 ((page_screen_w - new_sprite_w) / 2, (page_screen_h - new_sprite_h) / 2, 0);

		// Bring table position to top right corner, then to where it should be
		this.table.localPosition -= new Vector3 (screen_w/2 , screen_h / 2, 0);
		this.table.localPosition += new Vector3 (screen_w * 1.5f / 16, screen_h * 6f / 9);

	}

	public override bool pageCheck() {
		for (int i = 0; i < page.childCount; i++) {
			page.GetChild (i).GetComponent<Rune> ().findNeighbors ();
		}
		return true;
	}

	public void btnCheck() {
		Debug.Log (pageCheck ());
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
