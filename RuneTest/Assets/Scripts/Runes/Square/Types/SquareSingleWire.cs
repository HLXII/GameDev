using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareSingleWire : SquareRune {

	protected new void Start() {
		base.Start ();
		id += "Wire_Single_0";
		connections = new int[] { 0, 2 };
	}

}
