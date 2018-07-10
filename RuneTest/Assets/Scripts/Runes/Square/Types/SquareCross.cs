using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SquareCrossData : WireData {

	public SquareCrossData(int efficiency, int capacity) : base(efficiency,capacity) {
		id = "Cross";
	}

}

public class SquareCross : SquareRune {

	protected new void Start() {
		base.Start ();
		connections = new int[] { 0, 1, 2, 3 };
	}

}
