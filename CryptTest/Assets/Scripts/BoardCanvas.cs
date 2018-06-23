using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardCanvas : MonoBehaviour {

	public GameObject gem;

	protected int width = 2;
	protected int height = 2;

	protected virtual void setupBoard() {
		RectTransform b = transform.GetChild(0).transform as RectTransform;
		//Debug.Log (b.rect);

		RectTransform gemTransform = gem.transform as RectTransform;
		//Debug.Log (gemTransform.rect);

		Transform board = GameObject.Find ("Board").transform;

		float scale = Mathf.Min (b.rect.width / (4 * gemTransform.rect.width), b.rect.width / (width * gemTransform.rect.width));

		board.localScale = new Vector3 (scale, scale, 1);

		GameObject.Find ("Board").GetComponent<GridLayoutGroup> ().constraintCount = width;
	}

	// Use this for initialization
	public virtual void Start () {

		setupBoard ();

	}

	// Update is called once per frame
	void Update () {

	}
}
