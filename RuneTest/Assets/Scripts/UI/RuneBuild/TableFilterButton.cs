using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;



public class TableFilterButton : MonoBehaviour {

    public Sprite pressed;
    public Sprite unpressed;

	private bool state;

	// Use this for initialization
	void Start () {
		this.State = gameObject.name == "AllButton";
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
                gameObject.GetComponent<Image>().sprite = pressed;
			} else {
                gameObject.GetComponent<Image>().sprite = unpressed;
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
