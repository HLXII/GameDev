using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareSource : SquareRune {

	protected new void Start() {
		base.Start ();
		id += "Input_Source_0";
		connections = new int[] { 0 };
		outflow = new Energy[] { new Energy (10) };
	}

	public override void energyFlow ()
	{
		
	}

}
