using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareBlock : SquareRune {

	protected new void Start() {
		base.Start ();
		movable = false;
		swappable = false;
	}

}
