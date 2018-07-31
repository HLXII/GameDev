using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class SquareSinkData : OutputData {

	public SquareSinkData(int maxRate, int capacity, int outputRate) : base(maxRate, capacity, outputRate) {
		id = "Sink";
	}

}

public class SquareSink : SquareRune {

	private int storage;

	protected new void Start() {
		base.Start ();
		numConnections = 1;
		connections = new int[] { 0 };
		initEnergy ();

		storage = 0;
	}

	public override void reset ()
	{
		base.reset ();
		gameObject.GetComponent<Animator> ().Play ("off");
		gameObject.GetComponent<Animator> ().SetBool ("on", false);
		storage = 0;
	}

	public override void manipulateEnergy ()
	{
		// Releasing stored energy
		storage = Mathf.Max(0, storage - ((SquareSinkData)runeData).OutputRate);

		if (energyIn [0] != null) {
			// If input energy is less than max rate
			if (energyIn [0].Power <= ((SquareSinkData)runeData).MaxRate) {
				// Store inputted energy into storage
				storage += energyIn [0].Power;

				// If stored energy is greater than max capacity
				if (storage > ((SquareSinkData)runeData).Capacity) {
					signalReciever.receiveSignal ("Sink over max capacity");
					gameObject.GetComponent<Animator> ().SetTrigger ("error");
				}
				// Input over max rate
			} else {
				signalReciever.receiveSignal ("Sink receiving over max rate");
				gameObject.GetComponent<Animator> ().SetTrigger ("error");
			}
		}

		gameObject.GetComponent<Animator> ().SetBool ("on", (storage != 0));
	}


	public override string ToString () {
		string o = base.ToString ();
		o += "\nStorage: " + storage.ToString ();
		return o;
	}

	public override string getInfo ()
	{
		string o = "";
		o += runeData.ToString();
		o += "Storage: " + storage.ToString ();

		return o;
	}
}
