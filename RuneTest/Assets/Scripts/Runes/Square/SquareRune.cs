﻿//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//using System.Linq;

//public class SquareRune : Rune {

//	// Use this for initialization
//	protected new void Start () {
//		base.Start ();
//		sides = 4;
//	}

//	public override void findNeighbors() {

//		int page_h = dataManager.PageData.Page.GetLength (0);
//		int page_w = dataManager.PageData.Page.GetLength (1);

//		//Debug.Log ("H: " + page_h + " W: " + page_w);

//		int rune_idx = transform.GetSiblingIndex ();

//		int rune_x = rune_idx % page_w;
//		int rune_y = (rune_idx - rune_x) / page_w;

//		//Debug.Log ("X: " + rune_x + " Y: " + rune_y);

//		neighbors = new GameObject[4];

//		// There is a rune to the right
//		if (rune_x != page_w - 1) {
//			// The rune to the right is connected
//			GameObject rightRune = transform.parent.GetChild (rune_idx + 1).gameObject;
//			if (rightRune.GetComponent<Rune> ().isConnected (2)) {
//				neighbors [0] = rightRune;
//			} else {
//				neighbors [0] = null;
//			}
//		} else {
//			neighbors [0] = null;
//		}

//		// There is a rune above
//		if (rune_y != 0) {
//			// The rune above is connected
//			GameObject upRune = transform.parent.GetChild (rune_idx - page_w).gameObject;
//			if (upRune.GetComponent<Rune> ().isConnected (3)) {
//				neighbors [1] = upRune;
//			} else {
//				neighbors [1] = null;
//			}
//		} else {
//			neighbors [1] = null;
//		}

//		// There is a rune to the left
//		if (rune_x != 0) {
//			// The rune to the right is connected
//			GameObject rightRune = transform.parent.GetChild (rune_idx - 1).gameObject;
//			if (rightRune.GetComponent<Rune> ().isConnected (0)) {
//				neighbors [2] = rightRune;
//			} else {
//				neighbors [2] = null;
//			}
//		} else {
//			neighbors [2] = null;
//		}

//		// There is a rune below
//		if (rune_y != page_h - 1) {
//			// The rune below is connected
//			GameObject belowRune = transform.parent.GetChild (rune_idx + page_w).gameObject;
//			if (belowRune.GetComponent<Rune> ().isConnected (1)) {
//				neighbors [3] = belowRune;
//			} else {
//				neighbors [3] = null;
//			}
//		} else {
//			neighbors [3] = null;
//		}

//	}
//}
