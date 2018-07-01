using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareVoidData : RuneData {

	public SquareVoidData() {
		id = "S_Void";
		className = "SquareVoid";
	}

}

public class SquareVoid : SquareRune {

	protected new void Start() {
		base.Start ();
		movable = false;
		swappable = false;
	}

}
