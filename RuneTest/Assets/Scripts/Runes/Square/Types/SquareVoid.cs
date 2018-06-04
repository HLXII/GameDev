using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareVoid : SquareRune {

	protected new void Start() {
		base.Start ();
		id = "S_Special_Void_0";
		movable = false;
		swappable = false;
	}

}
