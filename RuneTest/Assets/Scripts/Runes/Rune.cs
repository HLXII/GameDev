using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Energy {

	private float power;

	public Energy(float power) {
		this.power = power;
	}

}

public class Rune : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

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

	public static string[] runeStrings = {
		"S_Special_Void_0",				//Square Void
		"S_Special_Empty_0",			//Square Empty
		"S_Speical_Block_0",			//Square Block
		"S_Wire_Single_0",				//Square Single Wire
		"S_Wire_Turn_0",				//Square Turning Wire
		"S_Wire_TJunction_0",			//Square T Junction Wire
		"S_Wire_FourWay_0",				//Square Four Way Intersection
		"S_Wire_Cross_0",				//Square Wire Crossing
		"S_Input_Source_0",				//Square Source
		"S_Output_Sink_0"				//Square Sink
	};

	protected string id;
	protected int sides;

	protected bool movable;
	protected bool swappable;

	protected int rotation;
	protected GameObject[] neighbors;
	protected Energy[] outflow;

	protected bool drag;
	protected Vector3 drag_start_position;
	protected Vector3 mouse_offset;

	protected void Start() {
		rotation = 0;
		sides = 4;
		movable = true;
		swappable = true;
	}

	protected void Update() {
		if (drag && Input.GetAxis ("Mouse ScrollWheel") > 0f) {
			rotation = (rotation + 1) % sides;
			Debug.Log ("Scroll Up "+rotation);
			transform.Rotate (Vector3.forward * 90);
		} else if (drag && Input.GetAxis ("Mouse ScrollWheel") < 0f) {
			
			rotation = (rotation - 1 + sides) % sides;
			transform.Rotate (Vector3.back * 90);
			Debug.Log ("Scroll Down "+rotation);
		}
	}

	void energyFlow() {

	}

	public int Rotation
	{
		get
		{
			return rotation;
		}
		set
		{
			rotation = value;
		}
	}

	public void drop() {
		Debug.Log ("Dropped Rune");

		BuildData buildData = GameObject.Find ("DataManager").GetComponent<DataManager> ().getBuildData ();

		int page_h = buildData.getPage ().GetLength (1);
		int page_w = buildData.getPage ().GetLength (0);

		// Dropping a page rune
		if (transform.parent.name == "Page") {
			Transform page = transform.parent;

			int grid_x = (int) Mathf.Floor (transform.localPosition.x);
			int grid_y = (int) Mathf.Floor (transform.localPosition.y);

			Debug.Log ("GRID:" + grid_x + " " + grid_y);

			// Checking if dropped onto page
			if (grid_x >= 0 & grid_x < page_w && grid_y >= 0 && grid_y < page_h) {

				int swap_idx = grid_x * page_h + grid_y;
				int cur_idx = transform.GetSiblingIndex ();

				// Getting rune to be swapped
				Rune swap_rune = page.GetChild (swap_idx).GetComponent<Rune> ();

				// Checking if the rune to be swapped is swappable, as well as if it is a different rune
				if (swap_rune.swappable && swap_idx != cur_idx) {
					
					Debug.Log ("SWAP: " + cur_idx + " " + swap_idx);

					transform.position = page.GetChild (swap_idx).transform.position;

					page.GetChild (swap_idx).transform.position = drag_start_position;

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

				//add stuff to table and update page, as well as adding a runeEmpty to the original slot
				//***
				transform.position = drag_start_position;
			}

		// Dropping a table rune
		} else {
			
			Transform page = GameObject.Find ("Page").transform;
			Vector3 grid_loc = (transform.position - page.position) / page.localScale.x;

			int grid_x = (int) Mathf.Floor (grid_loc.x);
			int grid_y = (int) Mathf.Floor (grid_loc.y);

			Debug.Log ("GRID:" + grid_x + " " + grid_y);

			// If rune was dropped onto page
			if (grid_x >= 0 & grid_x < page_w && grid_y >= 0 && grid_y < page_h) {

				int swap_idx = grid_x * page_h + grid_y;

				// Getting rune to be swapped
				Rune swap_rune = page.GetChild (swap_idx).GetComponent<Rune> ();

				// Checking if the rune to be swapped is swappable
				if (swap_rune.swappable) {
					Debug.Log ("REPLACE: "+swap_idx);

					//Replace and update table and page
					//***

				// Not swappable, return to table
				} else {
					StartCoroutine (shrinkAnimation ());
					transform.position = drag_start_position;
				}

			// Not dropped onto page, send back to table
			} else {
				StartCoroutine (shrinkAnimation ());
				transform.position = drag_start_position;
			}
		}
	}

	public virtual void findNeighbors() {
		Debug.Log ("Finding Neighbors");
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
				mouse_offset = transform.position - Camera.main.ScreenToWorldPoint (
					new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0)
				);
				this.drag = true;
			}
			if (transform.parent.name == "Table") {
				StartCoroutine (expandAnimation (GameObject.Find ("Page").transform.localScale));
			}
		}
	}

	public void OnDrag (PointerEventData eventData)
	{
		if (movable) {
			if (eventData.button == PointerEventData.InputButton.Left) {
				transform.position = Camera.main.ScreenToWorldPoint (
					new Vector3 (Input.mousePosition.x, Input.mousePosition.y, -1)
				) + mouse_offset;
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
		
}
