using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SquareSource : SquareRune {

	private bool on;

	protected new void Start() {
		base.Start ();
		connections = new int[] { 0 };
		energyIn = new Energy[1];
		energyOut = new Energy[1] {new Energy(10)};

		on = false;
	}

	public override void manipulateEnergy ()
	{
		if (energyIn [0] == null) {
			if (on) {
				//transform.GetChild (0).GetComponent<Animator> ().SetTrigger ("source");
				energyOut [0] = new Energy (10);
			}
		}
	}

	public override void OnPointerClick (PointerEventData eventData) {
		//Debug.Log (this);
		on = !on;
	}
}
