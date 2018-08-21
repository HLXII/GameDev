using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuneSelect : MonoBehaviour {

	private GameObject rune;
	private RuneSlot runeSlot;

	public GameObject Rune { get { return rune; }
		set {
			if (rune != null) {
                rune.GetComponent<Rune>().DeSelect();
			}
			rune = value; 
            runeSlot = rune.GetComponent<Rune> ().RuneSlot;
		} 
	}
    public RuneData RuneData { get { return (runeSlot == null) ? null : runeSlot.RuneData; } }

	private GameObject runeImage;
	private GameObject runeText;

	// Use this for initialization
	void Start () {
		runeImage = transform.GetChild (0).gameObject;
		runeText = transform.GetChild (1).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyDown (KeyCode.Escape)) {
			clearSelect ();
		}

		if (rune != null) {

			runeImage.GetComponent<Image> ().sprite = rune.GetComponent<Image>().sprite;

            //Debug.Log (runeImage.GetComponent<Image> ().sprite);

            runeText.GetComponent<Text>().text = runeSlot.GetInfo();

		}
	}

	public void clearSelect() {

		Debug.Log ("Clearing Selection");

		if (rune != null) {
            rune.GetComponent<Rune>().DeSelect();
			rune = null;
			runeImage.GetComponent<Image> ().sprite = null;
			runeText.GetComponent<Text> ().text = "";
			runeSlot = null;
		}
	}

}
