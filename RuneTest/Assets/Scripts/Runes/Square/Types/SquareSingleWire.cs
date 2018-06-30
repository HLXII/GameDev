using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SquareSingleWireData : RuneData {

	private int efficiency;
	private int capacity;

	public SquareSingleWireData() {
		id = "S_SingleWire";
		efficiency = 1;
		capacity = 1;
	}

	public SquareSingleWireData(int efficiency, int capacity) {
		id = "S_SingleWire";
		this.efficiency = efficiency;
		this.capacity = capacity;
	}

	public override string ToString () {
		string o = "";
		o += id + "\n";
		o += "Efficency: " + efficiency + "\n";
		o += "Capacity: " + capacity;
		return o;
	}

}

public class SquareSingleWire : SquareRune {

	private enum State {empty,left_in,right_in,both_in};

	private State prev_state;

	protected new void Start() {
		base.Start ();
		connections = new int[] { 0, 2 };
		prev_state = State.empty;
	}

	public override void reset() {
		base.reset ();
		prev_state = State.empty;
		gameObject.GetComponent<Animator> ().Play ("empty");
	}

	public override void manipulateEnergy ()
	{
		energyOut [1] = energyIn [0];
		energyOut [0] = energyIn [1];
		Debug.Log ("PREV: " + prev_state);

		// Energy coming in from both sides
		if (energyIn [0] != null && energyIn [1] != null) {
			//blow up?
			Debug.Log("EXPLODE");
			// Energy coming in from right
		} else if (energyIn [0] != null) {
			Debug.Log ("RIGHT");
			switch (prev_state) {
			case State.empty:
				gameObject.GetComponent<Animator> ().SetTrigger ("fill_right");
				break;
			case State.left_in:
				transform.GetChild (0).GetComponent<Animator> ().SetTrigger ("switch_to_left");
				break;
			case State.right_in:
				break;
			}
			prev_state = State.right_in;
			// Energy coming in from left
		} else if (energyIn [1] != null) {
			Debug.Log ("LEFT");
			switch (prev_state) {
			case State.empty:
				gameObject.GetComponent<Animator> ().SetTrigger ("fill_left");
				break;
			case State.left_in:
				break;
			case State.right_in:
				transform.GetChild (0).GetComponent<Animator> ().SetTrigger ("switch_to_right");
				break;
			}
			prev_state = State.left_in;
			// No energy coming in
		} else {
			switch (prev_state) {
			case State.empty:
				break;
			case State.left_in:
				gameObject.GetComponent<Animator> ().SetTrigger ("empty_left");
				break;
			case State.right_in:
				gameObject.GetComponent<Animator> ().SetTrigger ("empty_right");
				break;
			}
			prev_state = State.empty;
		}

		energyIn = new Energy[energyIn.Length];

	}

}
