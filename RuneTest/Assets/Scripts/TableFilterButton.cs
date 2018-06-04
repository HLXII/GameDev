using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;



public class TableFilterButton : MonoBehaviour {

	private bool state;

	// Use this for initialization
	void Start () {
		state = (gameObject.name == "btnClassAll" || gameObject.name == "btnRankAll");
		state = false;
		gameObject.GetComponent<Button> ().onClick.AddListener (this.onClick);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void onClick() {
		state = !state;
		Debug.Log (gameObject.name + " " + state);
	}

}
