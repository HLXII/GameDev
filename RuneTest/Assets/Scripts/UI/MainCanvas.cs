﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DataManager dm = GameObject.Find ("DataManager").GetComponent<DataManager> ();

		dm.loadSave (0);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
