using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Gem : MonoBehaviour, IPointerClickHandler {

	public static Dictionary<string, int> colorString = new Dictionary<string, int> ()
	{   {"white",0},																			
		{"black",1},
		{"red",2},
		{"orange",3},
		{"yellow",4},
		{"green",5},
		{"blue",6},
		{"purple",7},};

	public static Dictionary<int,Color> colorID = new Dictionary<int, Color> () 
	{   {0,new Color(1,1,1)},																			
		{1,new Color(.25f,.25f,.25f)},
		{2,new Color(1,0,0)},
		{3,new Color(1,.5f,0)},
		{4,new Color(1,1,0)},
		{5,new Color(0,.5f,0)},
		{6,new Color(0,0,1)},
		{7,new Color(1,0,1)},};

	public int gemColor;
	public int rimColor;

	public virtual void Start() {
		gameObject.GetComponent<Image> ().alphaHitTestMinimumThreshold = .5f;
	}


	public void setGem(int gemID) {
		gemColor = gemID;
		this.transform.GetChild (0).GetComponent<Image> ().color = colorID [gemColor];
		//Debug.Log (this.transform.GetChild (0).GetComponent<Image> ().color);
		//gameObject.GetComponent<Image> ().sprite = gemTypes [gemID];
			//Debug.Log ("Setting " + this.transform.position + " " + gameObject.GetComponent<Image> ().sprite.name);
		/*
		if (checkGem ()) {
			this.transform.GetChild (0).GetComponent<Animator> ().SetTrigger ("Sparkle");
		} else {
			this.transform.GetChild (0).GetComponent<Animator> ().SetTrigger ("Idle");
			Debug.Log ("????");
		}*/

	}

	public void setGem(string color) {
		gemColor = colorString [color];
		this.transform.GetChild (0).GetComponent<Image> ().color = colorID [gemColor];
	}

	public void setRim(int rimID) {
		rimColor = rimID;
		this.transform.GetChild (2).GetComponent<Image> ().color = colorID [rimColor];
	}

	public void setRim(string color) {
		rimColor = colorString[color];
		this.transform.GetChild (2).GetComponent<Image> ().color = colorID [rimColor];
	}

	public bool checkGem() {
		return (rimColor == gemColor);
	}

	public virtual void OnPointerClick (PointerEventData eventData) {
		Debug.Log (this);
	}
	/*
	public void ClickLight() {

		Transform board = GameObject.Find ("Board").transform;

		int size = (int)Mathf.Sqrt (board.transform.childCount);

		Vector3 position = this.transform.position;

		//int index = (int) (this.transform.position.x * size + this.transform.position.y);

		//Debug.Log("Position: " + this.transform.position + " Index: " + index);

		Vector3 up = this.transform.position;
		up.y = (up.y + 1) % size;
		Vector3 down = this.transform.position;
		down.y = (down.y - 1) % size;
		Vector3 left = this.transform.position;
		left.x = (left.x - 1) % size;
		Vector3 right = this.transform.position;
		right.x = (right.x + 1) % size;

		Debug.Log ("Up: " + up + " Down: " + down + " Left: " + left + " Right: " + right);

		

		GameObject gemMid = board.GetChild ((int)(position.x * size + position.y)).gameObject;
		GameObject gemUp = board.GetChild ((int)(position.x * size + (position.y + 1) % size)).gameObject;
		GameObject gemDown = board.GetChild ((int)(position.x * size + (position.y + size - 1) % size)).gameObject;
		GameObject gemLeft = board.GetChild ((int)(((position.x + size - 1) % size) * size + position.y)).gameObject;
		GameObject gemRight = board.GetChild ((int)(((position.x + 1) % size) * size + position.y)).gameObject;

		GameObject[] gems = new GameObject[] { gemMid, gemUp, gemDown, gemLeft, gemRight };

		for (int i = 0; i < gems.Length; i++) {
			
			// Gem is Black
			if (gems [i].GetComponent<Gem> ().gemColor == 0) {
				gems [i].GetComponent<Gem> ().setGem (1);
				// Gem is White
			} else {
				gems [i].GetComponent<Gem> ().setGem (0);
			}

		}

		GameObject.Find ("BoardManager").GetComponent<BoardManager> ().CheckComplete ();

	}*/

}
