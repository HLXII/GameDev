using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LightGem : Gem {

	public override void OnPointerClick (PointerEventData eventData) {
		Debug.Log ("Light");

	}

}
