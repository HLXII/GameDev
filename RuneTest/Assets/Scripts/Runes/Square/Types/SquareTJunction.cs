using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareTJunction : SquareRune {

	protected new void Start() {
		base.Start ();
		id += "Wire_TJunction_0";
		connections = new int[] { 0, 2, 3 }; 
	}

}
