using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSelectCanvas : BoardCanvas {

	// Use this for initialization
	public override void Start () {

		width = 2;
		height = 3;

		setupBoard ();

		Transform board = GameObject.Find ("Board").transform;

		for (int i = 0; i < width*height; i++) {
			GameObject instance = Instantiate (gem, board);
			SceneGem.Scene scene = (SceneGem.Scene)i + (int)SceneGem.Scene.LightSelect;
			instance.AddComponent<SceneGem> ();
			instance.GetComponent<SceneGem> ().SceneId = scene;
			if (PlayerPrefs.HasKey(scene.ToString())) {
				instance.GetComponent<Gem> ().RimColor = Gem.colorString["white"];
				if (PlayerPrefs.HasKey (scene.ToString () + "Complete")) {
					instance.GetComponent<Gem> ().GemColor =  Gem.colorString["white"];
				} else {
					instance.GetComponent<Gem> ().GemColor = Gem.colorString ["black"];
				}
			} else {
				instance.GetComponent<Gem> ().GemColor = Gem.colorString ["black"];
				instance.GetComponent<Gem> ().RimColor = Gem.colorString ["black"];
			}

		}

		setupBack (SceneGem.Scene.Title);

	}

	// Update is called once per frame
	void Update () {

	}
}
