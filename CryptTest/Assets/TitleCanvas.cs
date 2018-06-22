using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCanvas : MonoBehaviour {

	public GameObject gem;

	// Use this for initialization
	void Start () {
		
		RectTransform b = transform.GetChild(0).transform as RectTransform;
		Debug.Log (b.rect);

		RectTransform gemTransform = gem.transform as RectTransform;
		Debug.Log (gemTransform.rect);

		Transform board = GameObject.Find ("Board").transform;

		float scale = Mathf.Min (b.rect.width / (4 * gemTransform.rect.width), b.rect.width / (3 * gemTransform.rect.width));

		board.localScale = new Vector3 (scale, scale, 1);

		for (int i = 0; i < 4; i++) {
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
