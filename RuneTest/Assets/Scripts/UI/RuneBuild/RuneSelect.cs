using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuneSelect : MonoBehaviour {

	private GameObject rune;
	private RuneData runeData;

	public GameObject Rune { get { return rune; }
		set {
			if (rune != null) {
				rune.GetComponent<Rune> ().Selected = false;
			}
			rune = value; 
			rune.GetComponent<Rune> ().Selected = true;
			runeData = rune.GetComponent<Rune> ().RuneData;
		} 
	}
	public RuneData RuneData { get { return runeData; } }

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

			runeText.GetComponent<Text> ().text = rune.GetComponent<Rune> ().getInfo ();

		}
	}

	public void clearSelect() {

		Debug.Log ("Clearing Selection");

		if (rune != null) {
			rune.GetComponent<Rune> ().Selected = false;
			rune = null;
			runeImage.GetComponent<Image> ().sprite = null;
			runeText.GetComponent<Text> ().text = "";
			runeData = null;
		}
	}

}
