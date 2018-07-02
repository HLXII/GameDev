using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PalleteGem : Gem {

	public override void OnPointerClick (PointerEventData eventData) {
		for (int i = 0;i < transform.parent.childCount;i++) {
			transform.parent.GetChild(i).GetComponent<PalleteGem>().RimColor = colorString["black"];
		}
		RimColor = colorString["white"];
		transform.parent.parent.GetComponent<EditorCanvas>().CurrentColor = transform.GetSiblingIndex();
	}

}
