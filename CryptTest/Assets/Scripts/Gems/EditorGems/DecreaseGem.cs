﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class DecreaseGem : Gem {

	public override void OnPointerClick (PointerEventData eventData) {
		transform.parent.parent.GetComponent<EditorCanvas> ().decreaseSize ();
	}

}
