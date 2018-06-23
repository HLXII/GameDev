using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this);

		if (!PlayerPrefs.HasKey ("Exists")) {
			Debug.Log ("Creating new PlayerPrefs");
			PlayerPrefs.SetInt ("Exists", 0);




			PlayerPrefs.Save ();
		} else {
			Debug.Log ("PlayerPrefs exist");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public int[,,] loadSelect(string puzzleType) {
		return null;
	}

	public void loadPuzzle(string puzzleType, int puzzleId) {

	}

}
