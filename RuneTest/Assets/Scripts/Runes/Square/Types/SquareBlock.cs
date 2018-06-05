using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareBlock : SquareRune {

	protected new void Start() {
		base.Start ();
		id += "Special_Block_0";
		movable = false;
		swappable = false;
	}

}
