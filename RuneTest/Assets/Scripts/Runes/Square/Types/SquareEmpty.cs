using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SquareEmptyData : RuneData {

	public SquareEmptyData() {
		id = "S_Empty";
	}

}

public class SquareEmpty : SquareRune {

	protected new void Start() {
		base.Start ();
		movable = false;
	}

}
