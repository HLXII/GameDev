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

[System.Serializable]
public class EmptyData : RuneData {

	public EmptyData() {
		id = "Empty";
	}

}

public class Rune : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, ICanvasRaycastFilter {

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

	private GameObject previous_parent;
	private int previous_index;

	private bool active;

	protected void Start() {

		dataManager = GameObject.Find ("DataManager").GetComponent<DataManager> ();
		canvas = GameObject.Find ("Canvas").transform;
		table = GameObject.Find ("Table").transform.GetChild (0).GetChild (0).GetChild(0).transform;
		page = GameObject.Find ("Page").transform.GetChild (0).GetChild (0).transform;

		rotation = 0;
		sides = 4;
		movable = true;
		swappable = true;

		connections = new int[0];

		energyIn = new Energy[connections.Length];
		energyOut = new Energy[connections.Length];

		previous_parent = null;
		previous_index = 0;
		
		active = true;
	
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
	public bool Active { get { return active; } set { active = value; } } 

	public void drop() {

		Debug.Log ("Dropped Rune from " + previous_parent.name);

		// Dropping a page rune
		if (previous_parent.name == "PageContent") {

			Debug.Log ("Dropping Page Rune");

			// Raycasting to find where rune was dropped
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero, Mathf.Infinity, 1 << LayerMask.NameToLayer("Page Runes"));

			// Dropped onto a rune in page
			if (hit.collider != null && hit.collider.gameObject.transform.parent.name == "PageContent") {

				GameObject swap_rune = hit.collider.gameObject;

				Debug.Log ("Dropped onto " + hit.collider.gameObject.name);

				// Checking if the rune to be swapped is swappable, as well as if it is a different rune
				if (swap_rune.GetComponent<Rune> ().swappable && swap_rune.transform.GetSiblingIndex () != previous_index) {

					int new_index = swap_rune.transform.GetSiblingIndex ();

					transform.SetParent (page);
					transform.SetSiblingIndex (new_index);
					gameObject.layer = LayerMask.NameToLayer ("Page Runes");

					swap_rune.transform.SetSiblingIndex (previous_index);

					Destroy (page.GetChild (previous_index + 1).gameObject);

					// Rune can't be swapped, return to original position
				} else {

					Destroy (page.GetChild (previous_index).gameObject);

					transform.SetParent (page);
					transform.SetSiblingIndex (previous_index);
					gameObject.layer = LayerMask.NameToLayer ("Page Runes");

				}
				// Not dropped onto a rune in page, thus sent to table
			} else {

				Debug.Log ("Dropped off of page");

				canvas.GetComponent<BuildCanvas> ().addToTable (runeData);

				Destroy (gameObject);

			}
			// Dropping a table rune
		} else {

			Debug.Log ("Dropping Table Rune");

			// Raycasting to find where rune was dropped
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero, Mathf.Infinity, 1 << LayerMask.NameToLayer("Page Runes"));

			// Dropped onto a rune in page
			if (hit.collider != null && hit.collider.gameObject.transform.parent.name == "PageContent") {
				
				GameObject swap_rune = hit.collider.gameObject;

				Debug.Log ("Dropped onto " + hit.collider.gameObject.name);

				// Checking if the rune to be swapped is swappable, as well as if it is a different rune
				if (swap_rune.GetComponent<Rune> ().swappable) {

					canvas.GetComponent<BuildCanvas> ().removeFromTable (runeData);

					// If the swapped rune isn't empty, add it back to the table
					if (!swap_rune.GetComponent<Rune> ().Id.Contains ("Empty")) {
						canvas.GetComponent<BuildCanvas> ().addToTable (swap_rune.GetComponent<Rune> ().runeData);
					}

					transform.SetParent (page);
					transform.SetSiblingIndex (swap_rune.transform.GetSiblingIndex ());
					gameObject.layer = LayerMask.NameToLayer ("Page Runes");

					Destroy (page.GetChild (transform.GetSiblingIndex () + 1).gameObject);

					// Not swappable, return to table
				} else {
					//StartCoroutine (shrinkAnimation ());
					Destroy (table.GetChild (previous_index).gameObject);

					transform.SetParent (table);
					transform.SetSiblingIndex (previous_index);
					gameObject.layer = LayerMask.NameToLayer ("Table Runes");

					Rotation = 0;
				}
				// Not dropped onto page, send back to table
			} else {
				Destroy (table.GetChild (previous_index).gameObject);

				transform.SetParent (table);
				transform.SetSiblingIndex (previous_index);
				gameObject.layer = LayerMask.NameToLayer ("Table Runes");

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
		Vector3 new_scale = new Vector3 (Screen.width / 1600f,Screen.width / 1600f,1);
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

		// Checking if left button click
		if (eventData.button != PointerEventData.InputButton.Left) {
			return;
		}

		if (movable) {

			// Storing previous location data
			previous_parent = transform.parent.gameObject;
			previous_index = transform.GetSiblingIndex ();

			// Creating empty rune to hold position
			GameObject empty = Instantiate (canvas.GetComponent<BuildCanvas> ().runeEmpty, transform.parent);
			empty.GetComponent<Rune> ().RuneData = new EmptyData ();
			empty.transform.SetSiblingIndex (transform.GetSiblingIndex ());
			if (previous_parent.name == "TableContent") {
				empty.layer = LayerMask.NameToLayer ("Table Runes");
			} else if (previous_parent.name == "PageContent") {
				empty.layer = LayerMask.NameToLayer ("Page Runes");
			}

			// Initializing dragging
			drag_start_position = transform.position;
			mouse_offset = transform.position - new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0);
			mouse_offset = Vector3.zero;
			this.drag = true;

			// Start size transition animation
			if (previous_parent.name == "TableContent") {
				StartCoroutine (shrinkAnimation ());
			}

			// Setting parent and layer data
			transform.SetParent (canvas);
			gameObject.layer = LayerMask.NameToLayer ("Generic Runes");
		}
	}

	public void OnDrag (PointerEventData eventData)
	{

		// Checking if left button click
		if (eventData.button != PointerEventData.InputButton.Left) {
			return;
		}

		if (movable) {
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			transform.position = new Vector3(mousePos.x, mousePos.y, 0) + mouse_offset;
		}
	}

	public void OnEndDrag (PointerEventData eventData)
	{

		// Checking if left button click
		if (eventData.button != PointerEventData.InputButton.Left) {
			return;
		}

		if (movable) {
			transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0) + mouse_offset;
		}

		mouse_offset = Vector3.zero;
		this.drag = false;
		gameObject.GetComponent<Rune> ().drop ();
		StopAllCoroutines ();
		transform.localScale = new Vector3 (1, 1, 1);

		previous_parent = null;
		previous_index = 0;

	}

	public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
	{
		return active;
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
