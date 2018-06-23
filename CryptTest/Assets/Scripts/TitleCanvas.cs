using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCanvas : BoardCanvas {
	
	// Use this for initialization
	public override void Start () {

		setupBoard ();

		Transform board = GameObject.Find ("Board").transform;

		for (int i = 0; i < width*height; i++) {
			GameObject instance = Instantiate (gem, board);
			instance.AddComponent<SceneGem> ();
			switch (i) {
			case 0:
				instance.GetComponent<SceneGem> ().SceneId = SceneGem.Scene.PuzzleSelect;
				break;
			case 1:
				instance.GetComponent<SceneGem> ().SceneId = SceneGem.Scene.PuzzleSelect;
				break;
			case 2:
				instance.GetComponent<SceneGem> ().SceneId = SceneGem.Scene.PuzzleSelect;
				break;
			case 3:
				instance.GetComponent<SceneGem> ().SceneId = SceneGem.Scene.PuzzleSelect;
				break;
			default:
				break;
			}
		}
			
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
