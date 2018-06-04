using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareRune : Rune {

	// Use this for initialization
	protected new void Start () {
		base.Start ();
		id = "S";
		rotation = 0;
		sides = 4;
	}

	public override void findNeighbors() {
		Debug.Log ("Finding Square Neighbors");
	}
}
