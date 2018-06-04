using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareEmpty : SquareRune {

	protected new void Start() {
		base.Start ();
		id = "Special_Empty_0";
		movable = false;
	}

}
