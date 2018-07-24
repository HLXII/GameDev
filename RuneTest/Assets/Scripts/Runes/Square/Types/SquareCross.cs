using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class SquareCrossData : WireData {

	public SquareCrossData(int efficiency, int capacity) : base(efficiency,capacity) {
		id = "Cross";
	}

}

public class SquareCross : SquareRune {

	protected new void Start() {
		base.Start ();
		numConnections = 4;
		connections = new int[] { 0, 1, 2, 3 };
		initEnergy ();
	}

	public override void manipulateEnergy ()
	{
		if (energyIn [0] != null && energyIn [2] != null || energyIn [1] != null && energyIn [3] != null) {
			Debug.Log ("Wire receiving energy from both ports");
			signalReciever.receiveSignal ("Wire receiving energy from both ports");
		} else {
			if (energyIn[0] != null) {
				if (energyIn [0].Power > ((SquareCrossData)runeData).Capacity) {
					Debug.Log ("Wire over max capacity");
					signalReciever.receiveSignal ("Wire over max capacity");
				} else {
					energyIn [0].Power -= ((SquareCrossData)runeData).Loss;
					if (energyIn [0].Power <= 0) {
						energyOut [2] = null;
					} else {
						energyOut [2] = energyIn [0];
					}
				}
			} else if (energyIn[2] != null) {
				if (energyIn [2].Power > ((SquareCrossData)runeData).Capacity) {
					Debug.Log ("Wire over max capacity");
					signalReciever.receiveSignal ("Wire over max capacity");
				} else {
					energyIn [2].Power -= ((SquareCrossData)runeData).Loss;
					if (energyIn [2].Power <= 0) {
						energyOut [0] = null;
					} else {
						energyOut [0] = energyIn [2];
					}
				}
			}

			if (energyIn[1] != null) {
				if (energyIn [1].Power > ((SquareCrossData)runeData).Capacity) {
					Debug.Log ("Wire over max capacity");
					signalReciever.receiveSignal ("Wire over max capacity");
				} else {
					energyIn [1].Power -= ((SquareCrossData)runeData).Loss;
					if (energyIn [1].Power <= 0) {
						energyOut [3] = null;
					} else {
						energyOut [3] = energyIn [1];
					}
				}
			} else if (energyIn[3] != null) {
				if (energyIn [3].Power > ((SquareCrossData)runeData).Capacity) {
					Debug.Log ("Wire over max capacity");
					signalReciever.receiveSignal ("Wire over max capacity");
				} else {
					energyIn [3].Power -= ((SquareCrossData)runeData).Loss;
					if (energyIn [3].Power <= 0) {
						energyOut [1] = null;
					} else {
						energyOut [1] = energyIn [3];
					}
				}
			}
		}

		clearEnergyIn ();

	}

	public override void updateInfoPanel ()
	{
		string o = "";
		o += runeData.ToString();
		o += "\nOutput 1: ";
		if (energyOut [0] != null) {
			o += energyOut [0].ToString ();
		} else if (energyOut [2] != null) {
			o += energyOut [2].ToString ();
		}
		o += "\nOutput 2: ";
		if (energyOut [1] != null) {
			o += energyOut [1].ToString ();
		} else if (energyOut [3] != null) {
			o += energyOut [3].ToString ();
		}
		transform.GetChild (0).GetChild (0).GetComponent<Text> ().text = o;
	}

}
