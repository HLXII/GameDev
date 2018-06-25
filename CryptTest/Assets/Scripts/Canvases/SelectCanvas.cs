using System.Collections;
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

			if (PlayerPrefs.HasKey (puzzleType + i.ToString ())) {
				instance.GetComponent<PuzzleGem> ().setRim ("white");
			} else {
				instance.GetComponent<PuzzleGem> ().setRim ("black");
			}

			if (PlayerPrefs.HasKey (puzzleType + i.ToString () + "Complete")) {
				instance.GetComponent<PuzzleGem> ().setGem ("white");
			} else {
				instance.GetComponent<PuzzleGem> ().setGem ("black");
			}
		}

		setupBack (SceneGem.Scene.PuzzleSelect);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
