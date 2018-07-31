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
				instance.AddComponent<IncreaseGem> ();
				instance.GetComponent<IncreaseGem> ().GemColor = Gem.colorString["red"];
				break;
			case 1:
				instance.AddComponent<ToggleGem> ();
				instance.GetComponent<ToggleGem> ().GemColor = Gem.colorString["white"];
				instance.GetComponent<ToggleGem> ().RimColor = Gem.colorString["black"];
				break;
			case 2:
				instance.AddComponent<DecreaseGem> ();
				instance.GetComponent<DecreaseGem> ().GemColor = Gem.colorString["blue"];
				break;
			}
		}

	}

	public void increaseSize() {

		Debug.Log ("Increasing Size");

		width++;
		height++;
		setupBoard ();

		Transform board = GameObject.Find ("Board").transform;

		for (int i = 0; i < width-1; i++) {
			GameObject instance = Instantiate (gem, board);
			instance.AddComponent<EditorGem> ();
		}

		for (int i = width-1; i < width*height; i = i + width) {
			GameObject instance = Instantiate (gem, board);
			instance.AddComponent<EditorGem> ();
			instance.transform.SetSiblingIndex (i);
			Debug.Log (i);
		}


	}

	public void decreaseSize() {

		Debug.Log ("Decreasing Size");

		width--;
		height--;
		setupBoard ();

		Transform board = GameObject.Find ("Board").transform;

		for (int i = (width + 1) * (height + 1) - 1; i >= (width + 1) * (height + 1) - 1 - width; i--) {
			Destroy (board.GetChild (i).gameObject);
			Debug.Log (i);
		}

		for (int i = (width + 1) * (height + 1) - 1 - width - 1; i > 0; i = i - width - 1) {
			Destroy (board.GetChild (i).gameObject);
			Debug.Log (i);
		}

	}

	private void setupLeft() {
		Transform left = GameObject.Find("Left").transform;
		Transform board = GameObject.Find ("Board").transform;

		left.localScale = new Vector3 (MaxScale / 2, MaxScale / 2, 1);

		GameObject leftGem = Instantiate (gem, left);
		leftGem.AddComponent<SaveGem> ();
	}

	private void setupRight() {
		Transform right = GameObject.Find("Right").transform;
		Transform board = GameObject.Find ("Board").transform;

		right.localScale = new Vector3 (MaxScale / 2, MaxScale / 2, 1);

		GameObject rightGem = Instantiate (gem, right);
		rightGem.AddComponent<ClearGem> ();
	}

	// Update is called once per frame
	void Update () {
		
	}
}
