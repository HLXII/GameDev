using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class PageScrollRect : ScrollRect {

	private float minScale;
	private float maxScale;

	public new void Start() {
		base.Start();

		minScale = Screen.width / 1600f;
		maxScale = minScale * 2;
	}

	public override void OnScroll(PointerEventData data)
	{
		if (!IsActive())
			return;

		float newScale = content.localScale.x + data.scrollDelta.y * .05f;
		if (newScale < minScale) {
			newScale = minScale;
		}
		if (newScale > maxScale) {
			newScale = maxScale;
		}

		content.localScale = new Vector3 (newScale, newScale, 1);
	}

}

