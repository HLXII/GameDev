using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class SquareSingleWireData : WireData {

	public SquareSingleWireData(int loss, int capacity) : base(loss,capacity) {
		id = "SingleWire";
	}

}

public class SquareSingleWire : SquareRune {

	protected new void Start() {
		base.Start ();
		numConnections = 2;
		connections = new int[] { 0, 2 };
		initEnergy ();
	}

	public override void reset() {
		base.reset ();
		gameObject.GetComponent<Animator> ().Play ("empty");
	}

	public override void manipulateEnergy ()
	{
		if (energyIn [0] != null && energyIn [1] != null) {
			Debug.Log ("Wire receiving energy from both ports");
			signalReciever.receiveSignal ("Wire receiving energy from both ports");
			gameObject.GetComponent<Animator> ().SetTrigger ("error");
		} else {
			if (energyIn [0] != null) {
				if (energyIn [0].Power > ((SquareSingleWireData)runeData).Capacity) {
					Debug.Log ("Wire over max capacity");
					signalReciever.receiveSignal ("Wire over max capacity");
					gameObject.GetComponent<Animator> ().SetTrigger ("error");
				} else {
					energyIn [0].Power -= ((SquareSingleWireData)runeData).Loss;
					if (energyIn [0].Power <= 0) {
						energyOut [1] = null;
					} else {
						energyOut [1] = energyIn [0];
					}
					gameObject.GetComponent<Animator> ().SetBool ("outputting", true);
				}
			} else if (energyIn [1] != null) {
				if (energyIn [1].Power > ((SquareSingleWireData)runeData).Capacity) {
					Debug.Log ("Wire over max capacity");
					signalReciever.receiveSignal ("Wire over max capacity");
					gameObject.GetComponent<Animator> ().SetTrigger ("error");
				} else {
					energyIn [1].Power -= ((SquareSingleWireData)runeData).Loss;
					if (energyIn [1].Power <= 0) {
						energyOut [0] = null;
					} else {
						energyOut [0] = energyIn [1];
					}
					gameObject.GetComponent<Animator> ().SetBool ("outputting", true);
				}
			} else {
				gameObject.GetComponent<Animator> ().SetBool ("outputting", false);
			}
		}

		/*
		energyOut [1] = energyIn [0];
		energyOut [0] = energyIn [1];

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
		}*/

		clearEnergyIn ();

	}

	public override void updateInfoPanel ()
	{
		string o = "";
		o += runeData.ToString();
		o += "\nOutput: ";
		if (energyOut [0] != null) {
			o += energyOut [0].ToString ();
		} else if (energyOut [1] != null) {
			o += energyOut [1].ToString ();
		}

		transform.GetChild (0).GetChild (0).GetComponent<Text> ().text = o;

	}
}
