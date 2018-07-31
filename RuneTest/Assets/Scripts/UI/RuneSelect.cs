using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuneSelect : MonoBehaviour {

	private GameObject rune;
	public GameObject Rune { get { return rune; } set { rune = value; } }

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
			// deselect rune, have to run runes deselection script
			rune = null;
			runeImage.GetComponent<Image> ().sprite = null;
			runeText.GetComponent<Text> ().text = "";
		}

		if (rune != null) {

			runeImage.GetComponent<Image> ().sprite = rune.GetComponent<Image>().sprite;

			runeText.GetComponent<Text> ().text = rune.GetComponent<Rune> ().getInfo ();

		}
	}

}
