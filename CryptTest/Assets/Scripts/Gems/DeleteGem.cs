using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class DeleteGem : Gem {
	
	public override void Start() {
		gameObject.GetComponent<Image> ().alphaHitTestMinimumThreshold = .5f;
		setGem ("red");
		setRim ("black");
	}

	public override void OnPointerClick (PointerEventData eventData) {
		Debug.Log ("Deleting PlayerPrefs");

		PlayerPrefs.DeleteAll ();

		SceneManager.LoadScene ("Title");
	}

}
