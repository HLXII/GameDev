using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Linq;

public class Energy {

	private int power;

	public Energy(int power) {
		this.power = power;
	}

	public int Power { get { return power; } set { power = value; } }

	public override string ToString() {
		string o = "";
		o += power.ToString ();
		return o;
	}

}

/// <summary>
/// Rune data to be stored in files and used to initialize the rune GameObjects
/// </summary>
[System.Serializable]
public class RuneData {

	protected string id;

	public RuneData() {
	}

	public string Id {get{return id;} }

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
[System.Serializable]
public class BlockData : RuneData {
	public BlockData() {
		id = "Block";
	}
}
[System.Serializable]
public class VoidData : RuneData {
	public VoidData() {
		id = "Void";
	}
}
[System.Serializable]
public class WireData : RuneData {

	protected int loss;
	protected int capacity;
	public int Loss { get { return loss; } }
	public int Capacity { get { return capacity; } }

	public WireData(int loss, int capacity) {
		this.loss = loss;
		this.capacity = capacity;
	}

	public override string ToString () {
		string o = "";
		o += id + "\n";
		o += "Loss: " + loss + "\n";
		o += "Capacity: " + capacity;
		return o;
	}

}

[System.Serializable]
public class InputData : RuneData {

	protected int inputRate;
	public int InputRate { get { return inputRate; } }

	public InputData(int inputRate) {
		this.inputRate = inputRate;
	}

	public override string ToString () {
		string o = "";
		o += id + "\n";
		o += "Input Rate: " + inputRate;
		return o;
	}

}

[System.Serializable]
public class OutputData : RuneData {

	protected int maxRate;
	protected int capacity;
	protected int outputRate;
	public int MaxRate { get { return maxRate; } }
	public int Capacity { get { return capacity; } }
	public int OutputRate{ get { return outputRate; } }

	public OutputData(int maxRate, int capacity, int outputRate) {
		this.maxRate = maxRate;
		this.capacity = capacity;
		this.outputRate = outputRate;
	}

	public override string ToString () {
		string o = "";
		o += id + "\n";
		o += "Max Rate: " + maxRate + "\n";
		o += "Capacity: " + capacity + "\n";
		o += "Output Rate: " + outputRate + "\n";
		return o;
	}

}

/// <summary>
/// The Base Rune GameObject
/// </summary>
public class Rune : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerExitHandler, ICanvasRaycastFilter{

	// Info Panel GameObject
	public GameObject infoPanel;

	// RuneData to store the specific instance of the rune
	protected RuneData runeData;

	// Number of sides of the rune (Triangular, Square, Hexagonal
	protected int sides;

	// If rune is active/enabled
	protected bool active;

	// If rune is movable/swappable
	protected bool movable;
	protected bool swappable;

	protected int rotation;
	protected int numConnections;
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

	protected BuildSignalManager signalReciever;

	private GameObject previous_parent;
	private int previous_index;

	protected void Start() {

		dataManager = GameObject.Find ("DataManager").GetComponent<DataManager> ();
		canvas = GameObject.Find ("Canvas").transform;
		table = GameObject.Find ("Table").transform.GetChild (0).GetChild (0).GetChild(0).transform;
		page = GameObject.Find ("Page").transform.GetChild (0).GetChild (0).transform;

		movable = true;
		swappable = true;

		active = true;

		numConnections = 0;
		connections = new int[numConnections];
		initEnergy ();

		previous_parent = null;
		previous_index = 0;
	
	}

	protected void initEnergy() {
		energyIn = new Energy[numConnections];
		energyOut = new Energy[numConnections];
	}

	protected void clearEnergyIn() {
		for (int i = 0; i < numConnections; i++) {
			energyIn [i] = null;
		}
	}

	protected void clearEnergyOut() {
		for (int i = 0; i < numConnections; i++) {
			energyOut [i] = null;
		}
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

		if (transform.childCount > 0) {
			updateInfoPanel ();
		}
	}

	public RuneData RuneData { get { return runeData; } set { runeData = value; } }
	public string Id { get { return runeData.Id; } }
	public int Sides { get { return sides; } }
	public bool Active { get {return active; } set {active = value; } }
	public int Rotation { get { return rotation; } set {rotation = value; } }
	public GameObject[] Neighbors { get { return neighbors; } }
	public int[] Connections { get { return connections; } }
	public Energy[] EnergyIn { get { return energyIn; } }
	public Energy[] EnergyOut { get { return energyOut; } } 
	public BuildSignalManager SignalReceiver { get { return signalReciever; } set { signalReciever = value; } }

	public void drop() {

		//Debug.Log ("Dropped Rune from " + previous_parent.name);

		// Dropping a page rune
		if (previous_parent.name == "PageContent") {

			//Debug.Log ("Dropping Page Rune");

			// Raycasting to find where rune was dropped
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero, Mathf.Infinity, 1 << LayerMask.NameToLayer("Page Runes"));

			// Dropped onto a rune in page
			if (hit.collider != null && hit.collider.gameObject.transform.parent.name == "PageContent") {

				GameObject swap_rune = hit.collider.gameObject;

				//Debug.Log ("Dropped onto " + hit.collider.gameObject.name);

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

				//Debug.Log ("Dropped off of page");

				canvas.GetComponent<BuildCanvas> ().addToTable (runeData);

				Destroy (gameObject);

			}
			// Dropping a table rune
		} else {

			//Debug.Log ("Dropping Table Rune");

			// Raycasting to find where rune was dropped
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero, Mathf.Infinity, 1 << LayerMask.NameToLayer("Page Runes"));

			// Dropped onto a rune in page
			if (hit.collider != null && hit.collider.gameObject.transform.parent.name == "PageContent") {
				
				GameObject swap_rune = hit.collider.gameObject;

				//Debug.Log ("Dropped onto " + hit.collider.gameObject.name);

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
					Destroy (table.GetChild (previous_index).gameObject);

					transform.SetParent (table);
					transform.SetSiblingIndex (previous_index);
					gameObject.layer = LayerMask.NameToLayer ("Table Runes");

					Rotation = 0;
					transform.rotation = Quaternion.identity;
				}
				// Not dropped onto page, send back to table
			} else {
				Destroy (table.GetChild (previous_index).gameObject);

				transform.SetParent (table);
				transform.SetSiblingIndex (previous_index);
				gameObject.layer = LayerMask.NameToLayer ("Table Runes");

				Rotation = 0;
				transform.rotation = Quaternion.identity;
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
		clearEnergyOut();

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
		clearEnergyIn ();
		clearEnergyOut ();
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

	public virtual void updateInfoPanel() {}

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

	private IEnumerator shrinkAnimation (Vector3 scale) {
		Vector3 original_scale = transform.localScale;
		Vector3 new_scale = new Vector3 (Screen.width / 1600f,Screen.width / 1600f,1);
		new_scale = scale;
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
				StartCoroutine (shrinkAnimation (page.localScale));
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

		if (transform.childCount > 0) {
			Destroy (transform.GetChild (0).gameObject);
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

	public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera) {
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

		if (eventData.button == PointerEventData.InputButton.Right) {
			if (transform.childCount == 0 && infoPanel != null) {
				if (transform.parent.name == "PageContent") {
					GameObject instance = Instantiate (infoPanel, transform);
					instance.transform.Rotate (Vector3.forward * 90 * -rotation);
				} else if (transform.parent.name == "TableContent") {
					GameObject instance = Instantiate (infoPanel, transform);
				}
			}
		}

	}

	public void OnPointerExit(PointerEventData eventData) {
		if (transform.childCount > 0) {
			Destroy (transform.GetChild (0).gameObject);
		}
	}
		
}
