using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCanvas : BoardCanvas {

	private int currentColor;
	public int CurrentColor { get { return currentColor; } set { currentColor = value; } }
	private int currentGem;
	public int CurrentGem { get { return currentGem; } set { currentGem = value; } }

	// Use this for initialization
	public override void Start() {

		width = 3;
		height = 3;

		setupBoard ();

		Transform board = GameObject.Find ("Board").transform;

		currentColor = 0;

		for (int i = 0; i < width*height; i++) {
			GameObject instance = Instantiate (gem, board);
			instance.AddComponent<EditorGem> ();
		}

		setupTools ();

		setupBack (SceneGem.Scene.Title);

	}

	public void setupTools() {

		Transform pallete = GameObject.Find ("Pallete").transform;
		Transform editorTools = GameObject.Find ("EditorTools").transform;

		Transform board = GameObject.Find ("Board").transform;

		pallete.localScale = new Vector3 (MaxScale / 2.1f, MaxScale / 2.1f, 1);
		editorTools.localScale = new Vector3 (MaxScale / 2.1f, MaxScale / 2.1f, 1);

		for (int i = 0; i < 8; i++) {
			GameObject instance = Instantiate (gem, pallete);
			instance.AddComponent<PalleteGem> ();
			instance.GetComponent<PalleteGem> ().GemColor = i;
			instance.GetComponent<PalleteGem> ().RimColor = (i==0) ? Gem.colorString["white"] : Gem.colorString ["black"];
		}

		for (int i = 0; i < 3; i++) {
			GameObject instance = Instantiate (gem, editorTools);

			switch (i) {
			case 0:
				break;
			case 1:
				instance.AddComponent<ToggleGem> ();
				instance.GetComponent<ToggleGem> ().GemColor = Gem.colorString["white"];
				instance.GetComponent<ToggleGem> ().RimColor = Gem.colorString["black"];
				break;
			case 2:
				break;
			}
		}

	}

	// Update is called once per frame
	void Update () {
		
	}
}
