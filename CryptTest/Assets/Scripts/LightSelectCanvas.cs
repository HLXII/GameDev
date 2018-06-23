using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSelectCanvas : SelectCanvas {

	// Use this for initialization
	public override void Start () {
		puzzleType = "Light";
		width = 6;
		height = 6;

		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
