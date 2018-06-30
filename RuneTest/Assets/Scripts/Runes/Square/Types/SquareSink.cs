using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareSink : SquareRune {

	protected new void Start() {
		base.Start ();
		connections = new int[] { 0 };
	}

}
