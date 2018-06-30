using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Linq;

public class Energy {

	private float power;

	public Energy(float power) {
		this.power = power;
	}

	public override string ToString() {
		string o = "";
		o += power.ToString ();
		return o;
	}

}

[System.Serializable]
public class RuneData {

	protected string id;
	protected string className;

	public RuneData() {
	}

	public string Id {get{return id;} }
	public string ClassName { get { return className; } }

	public override string ToString() {
		return id;
	}

}

public class Rune : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler {

	/* Dictionary containing all available runes in the table.
	 * Keys are the rune strings, values are the number of runes of that type
	 * 
	 * Rune Strings:
	 * '1'_'2'_'3'	1 - T, S, or H for Triangle, Square or Hexagonal Runes
	 * 				2 - Type of rune, aka Wire, Input, Output, etc.
	 * 				
	 * "S_Void" - Void, will not show up
	 * "S_Empty" - Open, can place runes in this slot
	 * "S_Block" - Blocked, shows up but cannot move/place runes
	 * "S_SWire" - A single wire
	 * "S_TWire" - A T Junction wire
	 * "S_FWire" - A Four Way Junction wire
	 * "S_Cross" - A Cross Intersection wire
	 * "S_Source" - A Source Rune
	 * "S_Sink" - A Sink Rune
	 * 
	 * 
	*/

	/* List of runeStrings, which are ids defining all possible runes
	 * Formatted as follows:
	 * 	'1'_'2'_'3'_'4'
	 * 		1 - T, S, or H, depending on whether it is a Triangle, Square or Hexagonal rune
	 *		2 - Class of rune, whether Special, Input, Wire, etc
	 *		3 - Type of rune, to further describe it
	 *		4 - Rank of rune, whether lower quality or such
	*/

	/*
	public static string[] runeStrings = {
		"S_Special_Void_0",				//Square Void
		"S_Special_Empty_0",			//Square Empty
		"S_Special_Block_0",			//Square Block
		"S_Wire_Single_0",				//Square Single Wire
		"S_Wire_Turn_0",				//Square Turning Wire
		"S_Wire_TJunction_0",			//Square T Junction Wire
		"S_Wire_FourWay_0",				//Square Four Way Intersection
		"S_Wire_Cross_0",				//Square Wire Crossing
		"S_Input_Source_0",				//Square Source
		"S_Output_Sink_0"				//Square Sink
	};*/

	protected RuneData runeData;

	protected int sides;

	protected bool movable;
	protected bool swappable;

	protected int rotation;
	protected int[] connections;
	protected GameObject[] neighbors;

	protected Energy[] energyIn;
	protected Energy[] energyOut;

	protected bool drag;
	protected Vector3 drag_start_position;
	protected Vector3 mouse_offset;

	protected DataManager dataManager;
	protected Transform canvas;
	protected Transform table;
	protected Transform page;

	protected void Start() {

		dataManager = GameObject.Find ("DataManager").GetComponent<DataManager> ();
		canvas = GameObject.Find ("Canvas").transform;
		table = GameObject.Find ("Table").transform.GetChild (0).GetChild (0).transform;
		page = GameObject.Find ("Page").transform.GetChild (0).GetChild (0).transform;

		rotation = 0;
		sides = 4;
		movable = true;
		swappable = true;

		connections = new int[0];

		energyIn = new Energy[connections.Length];
		energyOut = new Energy[connections.Length];

		//Instantiate (energyAnimator, transform);

		//float sprite_size = gameObject.GetComponent<SpriteRenderer> ().sprite.rect.height;

		//transform.GetChild (0).localScale = new Vector3 (100f / sprite_size, 100f / sprite_size, 1);
	}

	protected void Update() {
		if (drag && Input.GetAxis ("Mouse ScrollWheel") > 0f) {
			// Updating connection ports
			rotation = (rotation + 1 + sides) % sides;
			//Debug.Log ("Scroll Up "+rotation);
			transform.Rotate (Vector3.forward * 360 / sides);
		} else if (drag && Input.GetAxis ("Mouse ScrollWheel") < 0f) {
			// Updating connection ports
			rotation = (rotation - 1 + sides) % sides;
			transform.Rotate (Vector3.back * 360 / sides);
			//Debug.Log ("Scroll Down "+rotation);
		}
	}

	public RuneData RuneData { get { return runeData; } set { runeData = value; } }
	public int Rotation {
		get { return rotation; }
		set { rotation = value; }
	}
	public GameObject[] Neighbors { get { return neighbors; } }
	public string Id { get { return runeData.Id; } }
	public int Sides { get { return sides; } }
	public int[] Connections { get { return connections; } }
	public Energy[] EnergyIn { get { return energyIn; } }
	public Energy[] EnergyOut { get { return energyOut; } } 

	public void drop() {
		//Debug.Log ("Dropped Rune");

		int page_h = dataManager.getBuildData().getPage ().GetLength (1);
		int page_w = dataManager.getBuildData().getPage ().GetLength (0);

		// Dropping a page rune
		if (transform.parent.name == "Page") {
			Transform page = transform.parent;

			int grid_x = (int) Mathf.Floor (transform.localPosition.x);
			int grid_y = (int) Mathf.Floor (transform.localPosition.y);

			//Debug.Log ("GRID:" + grid_x + " " + grid_y);

			// Checking if dropped onto page
			if (grid_x >= 0 & grid_x < page_w && grid_y >= 0 && grid_y < page_h) {

				int swap_idx = grid_x * page_h + grid_y;
				int cur_idx = transform.GetSiblingIndex ();

				int cur_y = cur_idx % page_h;
				int cur_x = (cur_idx - cur_y) / page_w;

				// Getting rune to be swapped
				Rune swap_rune = page.GetChild (swap_idx).GetComponent<Rune> ();

				// Checking if the rune to be swapped is swappable, as well as if it is a different rune
				if (swap_rune.swappable && swap_idx != cur_idx) {

					//Debug.Log ("POS: " + grid_x + " " + grid_y + " , " + cur_x + " " + cur_y);
					//Debug.Log ("SWAP: " + cur_idx + " " + swap_idx);

					// Swapping runes in the DataManager
					dataManager.getBuildData ().swapOnPage (cur_x, cur_y, grid_x, grid_y);

					// Swapping Rune positions
					transform.position = page.GetChild (swap_idx).transform.position;
					page.GetChild (swap_idx).transform.position = drag_start_position;

					// Swapping transform indices
					if (swap_idx > cur_idx) {
						page.GetChild (swap_idx).SetSiblingIndex (cur_idx);
						page.GetChild (cur_idx + 1).SetSiblingIndex (swap_idx);
					} else {
						page.GetChild (cur_idx).SetSiblingIndex (swap_idx);
						page.GetChild (swap_idx + 1).SetSiblingIndex (cur_idx);
					}

				// Rune can't be swapped, return to original position
				} else {
					transform.position = drag_start_position;
				}
			// Dropped off of page, send back to table
			} else {

				//Debug.Log ("Dropped off page, send back to table");

				int cur_idx = transform.GetSiblingIndex();

				// Adding dropped rune to table
				dataManager.getBuildData ().addToTable (runeData);
				// Updating table in case rune appears in filters
				transform.parent.parent.GetComponent<BuildCanvas> ().changeTable ();

				// Adding rune Empty to original position and deleting dropped rune
				transform.parent.parent.GetComponent<BuildCanvas> ().removeRune(cur_idx,drag_start_position);
			}
		// Dropping a table rune
		} else {
			
			Transform page = GameObject.Find ("Page").transform;
			Vector3 grid_loc = (transform.position - page.position) / page.localScale.x;

			int grid_x = (int) Mathf.Floor (grid_loc.x);
			int grid_y = (int) Mathf.Floor (grid_loc.y);

			//Debug.Log ("GRID:" + grid_x + " " + grid_y);

			// If rune was dropped onto page
			if (grid_x >= 0 & grid_x < page_w && grid_y >= 0 && grid_y < page_h) {

				int swap_idx = grid_x * page_h + grid_y;

				// Getting rune to be swapped
				Rune swap_rune = page.GetChild (swap_idx).GetComponent<Rune> ();

				// Checking if the rune to be swapped is swappable
				if (swap_rune.swappable) {

					//Debug.Log ("REPLACE: "+swap_idx);

					// Removing dropped rune from table
					dataManager.getBuildData ().removeFromTable (runeData);

					/*
					// If the rune being replaced is not the empty Rune, add back to table
					if (swap_rune.Id.Split ('_') [2] != "Empty") {
						// Adding replaced rune to table
						dataManager.getBuildData ().addToTable (swap_rune.runeData);
					}*/
						
					// Replacing rune on page
					transform.parent.parent.GetComponent<BuildCanvas> ().replaceRune (swap_idx, page.GetChild (swap_idx).position, gameObject);

					// Updating table
					transform.parent.parent.GetComponent<BuildCanvas> ().changeTable ();


				// Not swappable, return to table
				} else {
					StartCoroutine (shrinkAnimation ());
					transform.position = drag_start_position;
					Rotation = 0;
				}

			// Not dropped onto page, send back to table
			} else {
				StartCoroutine (shrinkAnimation ());
				transform.position = drag_start_position;
				Rotation = 0;
			}
		}
	}

	public void sendEnergy() {

		// Looping through all energy output
		for (int i = 0; i < energyOut.Length; i++) {
			// If there is energy to be outputted
			if (energyOut [i] != null) {
				int outPort = (connections [i] + rotation) % sides;
				int neighborInPort = (outPort + sides / 2) % sides;
				// If there is a neighbor to output to
				if (neighbors [outPort] != null) {
					neighbors [outPort].GetComponent<Rune> ().receiveEnergy (energyOut [i], neighborInPort);
				// There is no neighbor to output to, cause some kind of malfunction? ***
				} else {

				}
			}
		}

		// Clear energyOut
		energyOut = new Energy[connections.Length];

	}

	public virtual void manipulateEnergy() {}

	public void receiveEnergy(Energy energyIn, int port) {
		for (int i = 0; i < connections.Length; i++) {
			if ((connections [i] + rotation) % sides == port) {
				this.energyIn [i] = energyIn;
				break;
			}
		}
	}

	// Finds connected neighbors
	public virtual void findNeighbors() {}

	// Checks if there is a connection in the direction
	public bool isConnected (int direction)
	{
		foreach (int connection in connections) {
			if (direction == (connection + rotation) % sides) {
				return true;
			}
		}
		return false;
	}

	// Resets the energy state
	public virtual void reset() {
		energyIn = new Energy[connections.Length];
		energyOut = new Energy[connections.Length];
	}

	public bool checkNeighbors() {
		//Debug.Log (id);
		foreach (int connection in connections) {
			//Debug.Log ("C " + connection + "ACTUAL: " + ((connection + rotation) % sides));
			if (neighbors [(connection + rotation) % sides] == null) {
				Debug.Log (Id);
				Debug.Log (rotation);
				foreach (GameObject n in neighbors) {
					Debug.Log (n);
				}
				//Debug.Log (neighbors[(connection + rotation) % sides]);
				return false;
			}
		}
		//Debug.Log ("GOOD");
		return true;
	}

	private IEnumerator expandAnimation (Vector3 new_scale) {
		Vector3 original_scale = transform.localScale;
		float rate = 10.0f;
		float t = 0.0f;
		while (t < 1.0) {
			t += Time.deltaTime * rate;
			transform.localScale = Vector3.Lerp(original_scale, new_scale, Mathf.SmoothStep(0.0f, 1.0f, t));
			yield return null;
		}
	}

	private IEnumerator shrinkAnimation () {
		Vector3 original_scale = transform.localScale;
		Vector3 new_scale = new Vector3 (1, 1, 1);
		float rate = 10.0f;
		float t = 0.0f;
		while (t < 1.0) {
			t += Time.deltaTime * rate;
			transform.localScale = Vector3.Lerp (original_scale, new_scale, Mathf.SmoothStep (0.0f, 1.0f, t));
			yield return null;
		}
	}

	public void OnBeginDrag (PointerEventData eventData)
	{
		if (movable) {
			
			if (eventData.button == PointerEventData.InputButton.Left) {
				drag_start_position = transform.position;
				mouse_offset = transform.position - new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0);
				this.drag = true;
			}
			if (transform.parent.name == "Table") {
				StartCoroutine (expandAnimation (GameObject.Find ("Page").transform.localScale));
			}
			transform.SetParent (canvas);
		}
	}

	public void OnDrag (PointerEventData eventData)
	{
		if (movable) {
			if (eventData.button == PointerEventData.InputButton.Left) {
				transform.position = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, -1) + mouse_offset;
			}
		}
	}

	public void OnEndDrag (PointerEventData eventData)
	{
		if (movable) {
			if (eventData.button == PointerEventData.InputButton.Left) {
				transform.position = Camera.main.ScreenToWorldPoint (
					new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0)
				) + mouse_offset;
				mouse_offset = Vector3.zero;
				this.drag = false;
				gameObject.GetComponent<Rune> ().drop ();
			}
		}
	}

	public override string ToString ()
	{
		string o = "";
		o += runeData.ToString ();
		o += "\nR: " + rotation.ToString();
		o += "\nConnections: ";
		foreach (int connection in connections) {
			o += connection.ToString () + " ";
		}
		o += "\nActual: ";
		foreach (int connection in connections) {
			o += ((connection+rotation) % sides).ToString () + " ";
		}
		o += "\nEnergyIn: ";
		foreach (Energy e in energyIn) {
			if (e == null) {
				o += "null ";
			} else {
				o += e.ToString () + " ";
			}
		}
		o += "\nEnergyOut: ";
		foreach (Energy e in energyOut) {
			if (e == null) {
				o += "null ";
			} else {
				o += e.ToString () + " ";
			}
		}
		if (neighbors != null) {
			o += "\nNeighbors: ";
			foreach (GameObject neighbor in neighbors) {
				if (neighbor == null) {
					o += "null ";
				} else {
					o += neighbor.GetComponent<Rune> ().Id + " ";
				}
			}
		}
		return o;
	}

	public virtual void OnPointerClick (PointerEventData eventData) {
		Debug.Log (this);
	}
		
}
