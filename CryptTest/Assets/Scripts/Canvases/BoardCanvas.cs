using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardCanvas : MonoBehaviour {

	private float maxScale;
	public float MaxScale { get { return maxScale; } }

	public GameObject gem;

	protected int width = 2;
	protected int height = 2;

	protected virtual void setupBoard() {
		RectTransform b = transform.GetChild(0).transform as RectTransform;
		//Debug.Log (b.rect);

		RectTransform gemTransform = gem.transform as RectTransform;
		//Debug.Log (gemTransform.rect);

		Transform board = GameObject.Find ("Board").transform;

		maxScale = b.rect.width / (4 * gemTransform.rect.width);

		float scale = Mathf.Min (maxScale, b.rect.width / (width * gemTransform.rect.width), b.rect.height / (height * gemTransform.rect.height));
		
		board.localScale = new Vector3 (scale, scale, 1);

		GameObject.Find ("Board").GetComponent<GridLayoutGroup> ().constraintCount = width;
	}

	protected void setupBack(SceneGem.Scene scene) {
		Transform back = GameObject.Find("Back").transform;
		Transform board = GameObject.Find ("Board").transform;

		back.localScale = new Vector3 (maxScale / 2, maxScale / 2, 1);

		GameObject backGem = Instantiate (gem, back);
		backGem.AddComponent<SceneGem> ();
		backGem.GetComponent<SceneGem> ().SceneId = scene;
	}

	protected void setupLeft(SceneGem.Scene scene) {
		Transform left = GameObject.Find("Left").transform;
		Transform board = GameObject.Find ("Board").transform;

		left.localScale = new Vector3 (maxScale / 2, maxScale / 2, 1);

		GameObject leftGem = Instantiate (gem, left);
		leftGem.AddComponent<SceneGem> ();
		leftGem.GetComponent<SceneGem> ().SceneId = scene;
	}

	protected void setupRight(SceneGem.Scene scene) {
		Transform right = GameObject.Find("Right").transform;
		Transform board = GameObject.Find ("Board").transform;

		right.localScale = new Vector3 (maxScale / 2, maxScale / 2, 1);

		GameObject rightGem = Instantiate (gem, right);
		rightGem.AddComponent<SceneGem> ();
		rightGem.GetComponent<SceneGem> ().SceneId = scene;
	}

	// Use this for initialization
	public virtual void Start () {

		setupBoard ();

	}

	// Update is called once per frame
	void Update () {

	}
}
