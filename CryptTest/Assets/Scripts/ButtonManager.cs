using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour {

	public GameObject Gem;

	// Use this for initialization
	void Start () {

		Transform canvas = GameObject.Find ("BoardManager").transform;

		this.transform.SetParent (canvas);

		/* three buttons
		 * left - previous puzzle
		 * 			white if it isn't hte first puzzle
		 * mid - puzzle selection
		 * 			always white
		 * 
		 * right - Next puzzle
		 * 			white if next puzle is avaialbe, 
		 * 			turns white whne complete the puzle
		*/
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
