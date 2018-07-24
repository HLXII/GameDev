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

		minScale = Screen.width / 3200f;
		maxScale = minScale * 4;
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
	/*
	public override void OnBeginDrag(PointerEventData eventData) {
		if (eventData.button == PointerEventData.InputButton.Right) {
			eventData.button = PointerEventData.InputButton.Left;
			base.OnBeginDrag (eventData);
		}
	}

	public override void OnDrag(PointerEventData eventData) {
		if (eventData.button == PointerEventData.InputButton.Right) {
			eventData.button = PointerEventData.InputButton.Left;
			base.OnDrag (eventData);
		}
	}

	public override void OnEndDrag(PointerEventData eventData) {
		if (eventData.button == PointerEventData.InputButton.Right) {
			eventData.button = PointerEventData.InputButton.Left;
			base.OnEndDrag (eventData);
		}
	}*/

}

