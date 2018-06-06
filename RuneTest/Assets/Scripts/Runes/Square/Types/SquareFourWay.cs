using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareFourWay : SquareRune {

	protected new void Start() {
		base.Start ();
		id += "Wire_FourWay_0";
		connections = new int[] { 0, 1, 2, 3 };
	}

}
