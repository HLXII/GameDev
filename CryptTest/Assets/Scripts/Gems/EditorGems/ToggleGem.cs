using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ToggleGem : Gem {

	public override void OnPointerClick (PointerEventData eventData) {
		GemColor = (GemColor + 1) % 2;
		RimColor = (RimColor + 1) % 2;
		transform.parent.parent.GetComponent<EditorCanvas> ().CurrentGem = (transform.parent.parent.GetComponent<EditorCanvas> ().CurrentGem + 1) % 2;
	}

}
