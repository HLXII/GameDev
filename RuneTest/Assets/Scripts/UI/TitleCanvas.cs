﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleCanvas : MonoBehaviour {

	// Use this for initialization
	void Start () {

		Debug.Log ("Checking " + Application.persistentDataPath);

		string[] saves = null;

		if (!Directory.Exists (Application.persistentDataPath + "/saves/")) {

			Debug.Log ("Save Directory does not exist, initializing.");

			Directory.CreateDirectory (Application.persistentDataPath + "/saves/");

		} else {

			Debug.Log ("Finding Saves");

			saves = Directory.GetFiles (Application.persistentDataPath + "/saves/", "*.sav");

		}

		if (saves == null || saves.Length == 0) {
			Debug.Log ("No saves");

			GameObject.Find ("Continue").GetComponent<Button> ().enabled = false;
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void loadScene(string scene) {
		SceneManager.LoadScene (scene);
	}
}
