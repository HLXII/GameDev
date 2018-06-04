using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject.Find ("DataManager").GetComponent<DataManager> ().loadData ("Square", "test");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
