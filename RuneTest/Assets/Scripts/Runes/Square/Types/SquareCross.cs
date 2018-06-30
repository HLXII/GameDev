using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareCross : SquareRune {

	protected new void Start() {
		base.Start ();
		connections = new int[] { 0, 1, 2, 3 };
	}

}
