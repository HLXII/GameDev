using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCanvas : BoardCanvas {

	// Use this for initialization
	public override void Start () {

		DataManager dataManager = GameObject.Find ("DataManager").GetComponent<DataManager> ();

		int[,,] data = dataManager.getData ();

		width = data.GetLength (0);
		height = data.GetLength (1);

		setupBoard ();

		Transform board = GameObject.Find ("Board").transform;

		for (int h = 0; h < height; h++) {

			for (int w = 0; w < width; w++) {

				GameObject instance = Instantiate (gem, board);
				instance.AddComponent<LightGem> ();
				instance.GetComponent<LightGem> ().GemColor =  data [h, w, 0];
				instance.GetComponent<LightGem> ().RimColor =  data [h, w, 1];

			}

		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void checkComplete() {
		Debug.Log (isComplete ());
	}

	public bool isComplete() {
		Transform board = transform.GetChild (0);
		for (int i = 0; i < board.childCount; i++) {
			if (!board.GetChild (i).GetComponent<LightGem> ().checkGem ()) {
				return false;
			}
		}
		return true;
	}
}
