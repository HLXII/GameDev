using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSelectCanvas : MonoBehaviour {

	public GameObject gem;

	// Use this for initialization
	void Start () {

		RectTransform b = transform.GetChild(0).transform as RectTransform;
		//Debug.Log (b.rect);

		RectTransform gemTransform = gem.transform as RectTransform;
		//Debug.Log (gemTransform.rect);

		float scale = Mathf.Min (b.rect.width / (4 * gemTransform.rect.width), b.rect.width / (3 * gemTransform.rect.width));

		//Debug.Log (scale);

		Transform board = GameObject.Find ("Board").transform;

		board.localScale = new Vector3 (scale, scale, 1);

		for (int i = 0; i < 6; i++) {
			GameObject instance = Instantiate (gem, board);
			instance.AddComponent<SceneGem> ();
			instance.GetComponent<SceneGem> ().SceneId = (SceneGem.Scene)i + (int) SceneGem.Scene.LightSelect;
		}

	}

	// Update is called once per frame
	void Update () {

	}
}
