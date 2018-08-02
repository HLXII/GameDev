using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;



public class TableFilterButton : MonoBehaviour {

	private bool state;

	// Use this for initialization
	void Start () {
		this.State = gameObject.name == "btnAll";
		gameObject.GetComponent<Button> ().onClick.AddListener (this.onClick);
	}
		
	public bool State
	{
		get
		{
			return state;
		}
		set
		{
			if (value) {
				gameObject.GetComponent<Image> ().color = new Color (0, 0, 0);
			} else {
				gameObject.GetComponent<Image> ().color = new Color (255, 255, 255);
			}
			state = value;
		}
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void onClick() {
		for (int i = 0; i < transform.parent.childCount; i++) {
			transform.parent.GetChild (i).GetComponent<TableFilterButton> ().State = false;
		}
		this.State = true;
	}

}
