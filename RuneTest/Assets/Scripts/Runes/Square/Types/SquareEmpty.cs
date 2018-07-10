using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareEmpty : SquareRune {

	protected new void Start() {
		base.Start ();
		movable = false;
		runeData = new EmptyData ();
	}

}
