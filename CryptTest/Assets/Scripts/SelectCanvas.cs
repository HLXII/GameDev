﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCanvas : BoardCanvas {

	protected string puzzleType = "None";

	// Use this for initialization
	public override void Start () {

		setupBoard ();
		Transform board = GameObject.Find ("Board").transform;

		for (int i = 0; i < width*height; i++) {
			GameObject instance = Instantiate (gem, board);
			instance.AddComponent<PuzzleGem> ();
			instance.GetComponent<PuzzleGem> ().PuzzleId = i;
			instance.GetComponent<PuzzleGem> ().PuzzleType = puzzleType;
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
