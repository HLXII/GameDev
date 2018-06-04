using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class SquareCanvas : MonoBehaviour {

	private Transform page;

	private Transform table;

	// Sprite to be placed behind rune slots that can be used
	public GameObject runeBack;

	// All possible runes to be placed
	public GameObject runeVoid;
	public GameObject runeEmpty;
	public GameObject runeBlock;
	public GameObject runeSource;
	public GameObject runeSink;
	public GameObject runeSingleWire;
	public GameObject runeTJunction;
	public GameObject runeFourWay;
	public GameObject runeCross;

	// Dictionary to instantiate the correct runes from the pageData
	private Dictionary<string,GameObject> runes;

	// List to store filtered and available runes
	//private List<string> runes;

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
			print (Input.mousePosition);
			print(Input.GetAxis("Mouse ScrollWheel"));
		}
	}

	// Initializing the rune dictionary
	private void initRunes() {
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
	private void initBuild() {
		
		Debug.Log ("Initializing Build");

		// Getting buildData from DataManager
		BuildData buildData = GameObject.Find ("DataManager").GetComponent<DataManager> ().getBuildData ();

		// Getting data from buildData

		Debug.Log ("Getting table");
		SortedList<string,int> tableData = buildData.getTable (new RuneByClass());
		Debug.Log ("Got table");
		string[,] pageData = buildData.getPage ();
		for (int i = 0; i < tableData.Count; i++) {
			Debug.Log (tableData.Keys[i]);
		}

		/*
		// Debug prints for table and page
		foreach (KeyValuePair<string, int> item in tableData)
		{
			Debug.Log(item.Key + ": " + item.Value.ToString());
		}

		for (int i = 0; i < pageData.GetLength (0); i++) {
			for (int j = 0; j < pageData.GetLength (1); j++) {
				Debug.Log (pageData [i, j]);
			}
		}*/

		// Creating the Table object to hold all the runes in the table
		table = new GameObject("Table").transform;
		table.SetParent (gameObject.transform);



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

		Debug.Log ("Page Screen: W" + page_screen_w + " H" + page_screen_h);

		// Getting the size of the rune tiles in the page transform by pixels
		float sprite_w = runeEmpty.GetComponent<SpriteRenderer> ().sprite.rect.width;
		float sprite_h = runeEmpty.GetComponent<SpriteRenderer> ().sprite.rect.height;

		Debug.Log ("Sprite Size: W" + sprite_w + " H" + sprite_h);

		float h_scale = page_screen_h / page_h;
		float w_scale = page_screen_w / page_w;

		Debug.Log ("HSCALE: " + h_scale + " WSCALE: " + w_scale);

		Debug.Log ("BEFORE: " + this.page.localPosition);

		// Bring page position to top left corner, then to where it should be
		this.page.localPosition -= new Vector3 (screen_w/2, screen_h/2, 0);
		this.page.localPosition += new Vector3 (screen_w*5/16, screen_h*3.5f/9, 0);

		float scale = Mathf.Min(2, (h_scale < w_scale) ? h_scale : w_scale);

		// Finding new sprite widths and heights with new scale
		float new_sprite_w = page_w * scale;
		float new_sprite_h = page_h * scale;

		this.page.localScale = new Vector3 (scale, scale, 1);
		this.page.localPosition += new Vector3 ((page_screen_w - new_sprite_w) / 2, (page_screen_h - new_sprite_h) / 2, 0);

		Debug.Log ("AFTER: " + this.page.localPosition);

		// Bring table position to top right corner, then to where it should be
		this.table.localPosition -= new Vector3 (screen_w/2 , screen_h / 2, 0);
		this.table.localPosition += new Vector3 (screen_w * 4.5f / 16, screen_h * .5f / 9);

	}

}
