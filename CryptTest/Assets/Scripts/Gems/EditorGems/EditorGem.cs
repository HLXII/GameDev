using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class EditorGem : Gem {

	public override void OnPointerClick (PointerEventData eventData) {

		// Getting EditorCanvas for current editor details
		EditorCanvas editor = transform.parent.parent.GetComponent<EditorCanvas> ();

		// Editing main gem
		if (editor.CurrentGem == 0) {
			GemColor = editor.CurrentColor;
		// Editing rim gem 
		} else {
			RimColor = editor.CurrentColor;
		}
	}

}
