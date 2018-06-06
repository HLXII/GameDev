using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareCross : SquareRune {

	protected new void Start() {
		base.Start ();
		id += "Wire_Cross_0";
		connections = new int[] { 0, 1, 2, 3 };
	}

}
