﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class SquareCornerData : WireData {

	public SquareCornerData(int efficiency, int capacity) : base(efficiency,capacity) {
		id = "Corner";
	}

}

public class SquareCorner : SquareRune {

	protected new void Start() {
		base.Start ();
		numConnections = 2;
		connections = new int[] { 0, 1 };
		initEnergy ();
	}
		
	public override void manipulateEnergy ()
	{
		if (energyIn [0] != null && energyIn [1] != null) {
			Debug.Log ("Wire receiving energy from both ports");
			signalReciever.receiveSignal ("Wire receiving energy from both ports");
		} else {
			if (energyIn[0] != null) {
				if (energyIn [0].Power > ((SquareCornerData)runeData).Capacity) {
					Debug.Log ("Wire over max capacity");
					signalReciever.receiveSignal ("Wire over max capacity");
				} else {
					energyIn [0].Power -= ((SquareCornerData)runeData).Loss;
					if (energyIn [0].Power <= 0) {
						energyOut [1] = null;
					} else {
						energyOut [1] = energyIn [0];
					}
				}
			} else if (energyIn[1] != null) {
				if (energyIn [1].Power > ((SquareCornerData)runeData).Capacity) {
					Debug.Log ("Wire over max capacity");
					signalReciever.receiveSignal ("Wire over max capacity");
				} else {
					energyIn [1].Power -= ((SquareCornerData)runeData).Loss;
					if (energyIn [1].Power <= 0) {
						energyOut [0] = null;
					} else {
						energyOut [0] = energyIn [1];
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
		o += "\nOutput: ";
		if (energyOut [0] != null) {
			o += energyOut [0].ToString ();
		} else if (energyOut [1] != null) {
			o += energyOut [1].ToString ();
		}

		transform.GetChild (0).GetChild (0).GetComponent<Text> ().text = o;

	}

}
