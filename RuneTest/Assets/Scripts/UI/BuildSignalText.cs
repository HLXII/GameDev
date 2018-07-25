using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildSignalText : MonoBehaviour, BuildSignalManager, IPointerEnterHandler, IPointerExitHandler {

	private int numLines;

	private Vector2 minAnchor;
	private Vector2 maxAnchor;

	// Use this for initialization
	void Start () {
		initialize ();

		minAnchor = ((RectTransform)transform).anchorMax;
		maxAnchor = new Vector2 (((RectTransform)transform).anchorMax.x, 6f / 9f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void reset() {
		numLines = 0;
		transform.GetChild (0).GetChild (0).GetComponent<Text> ().text = "";
	}

	public void OnPointerEnter(PointerEventData eventData) {
		Debug.Log("ENTER");
		StartCoroutine (expandAnimation());
	}

	public void OnPointerExit(PointerEventData eventData) {
		Debug.Log("EXIT");
		StartCoroutine (shrinkAnimation ());
	}

	public void initialize() {
		numLines = 1;
		transform.GetChild (0).GetChild (0).GetComponent<Text> ().text = "Starting Simulation";
	}

	public void receiveSignal(string signal) {
		numLines += 1;
		transform.GetChild (0).GetChild (0).GetComponent<Text> ().text += "\n" + signal;
	}

	private IEnumerator shrinkAnimation () {
		Vector2 original = ((RectTransform)transform).anchorMax;
		float rate = 5.0f;
		float t = 0.0f;
		while (t < 1.0) {
			t += Time.deltaTime * rate;
			((RectTransform)transform).anchorMax = Vector2.Lerp(original, minAnchor, Mathf.SmoothStep (0.0f, 1.0f, t));
			yield return null;
		}
	}

	private IEnumerator expandAnimation () {
		Vector2 original = ((RectTransform)transform).anchorMax;
		float rate = 5.0f;
		float t = 0.0f;
		while (t < 1.0) {
			t += Time.deltaTime * rate;
			((RectTransform)transform).anchorMax = Vector2.Lerp(original, maxAnchor, Mathf.SmoothStep (0.0f, 1.0f, t));
			yield return null;
		}
	}
}
