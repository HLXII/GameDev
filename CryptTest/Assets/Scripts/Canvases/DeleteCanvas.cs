using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteCanvas : BoardCanvas {

	// Use this for initialization
	public override void Start () {
		width = 1;
		height = 1;

		setupBoard ();

		Transform board = GameObject.Find ("Board").transform;

		GameObject instance = Instantiate (gem, board);
		instance.AddComponent<DeleteGem> ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
