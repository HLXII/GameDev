using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LightGem : Gem {

	public override void OnPointerClick (PointerEventData eventData) {

		int cur_idx = transform.GetSiblingIndex ();

		Transform board = transform.parent;

		int width = board.GetComponent<GridLayoutGroup> ().constraintCount;
		int height = board.childCount / width;

		int up = cur_idx - width;
		int down = cur_idx + width;
		int left = cur_idx - 1;
		int right = cur_idx + 1;

		board.GetChild (cur_idx).GetComponent<Gem> ().GemColor = (board.GetChild (cur_idx).GetComponent<Gem> ().GemColor + 1) % 2;

		if (up >= 0) {
			//Debug.Log ("UP");
			board.GetChild (up).GetComponent<Gem> ().GemColor = (board.GetChild (up).GetComponent<Gem> ().GemColor + 1) % 2;
		}

		if (down < width * height) {
			//Debug.Log ("DOWN");
			board.GetChild (down).GetComponent<Gem> ().GemColor = (board.GetChild (down).GetComponent<Gem> ().GemColor + 1) % 2;
		}

		if (left >= 0 && left % width != width - 1) {
			//Debug.Log ("LEFT");
			board.GetChild (left).GetComponent<Gem> ().GemColor = (board.GetChild (left).GetComponent<Gem> ().GemColor + 1) % 2;
		}

		if (right <= width * height && right % width != 0) {
			//Debug.Log ("RIGHT");
			board.GetChild (right).GetComponent<Gem> ().GemColor = (board.GetChild (right).GetComponent<Gem> ().GemColor + 1) % 2;
		}

		transform.parent.parent.GetComponent<LightCanvas> ().checkComplete ();
	}

}
