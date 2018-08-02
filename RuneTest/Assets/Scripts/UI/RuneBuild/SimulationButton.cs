using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SimulationButton : MonoBehaviour {

	private BuildCanvas buildCanvas;

	// Use this for initialization
	void Start () {

		buildCanvas = GameObject.Find ("Canvas").GetComponent<BuildCanvas> ();

		gameObject.GetComponent<Button> ().onClick.AddListener (simulate);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void simulate() {
		Debug.Log ("Simulating");

		buildCanvas.simulate ();

		gameObject.GetComponent<Button> ().onClick.RemoveAllListeners ();
		gameObject.GetComponent<Button> ().onClick.AddListener (endSimulate);
		transform.GetChild (0).GetComponent<Text> ().text = "End Simulation";
	}

	void endSimulate() {
		Debug.Log ("Ending Simulation");

		buildCanvas.endSimulate ();

		gameObject.GetComponent<Button> ().onClick.RemoveAllListeners ();
		gameObject.GetComponent<Button> ().onClick.AddListener (cleanSimulation);
		transform.GetChild (0).GetComponent<Text> ().text = "Clean Simulation";
	}

	void cleanSimulation() {
		Debug.Log ("Cleaning Simulation");

		buildCanvas.cleanSimulation ();

		gameObject.GetComponent<Button> ().onClick.RemoveAllListeners ();
		gameObject.GetComponent<Button> ().onClick.AddListener (simulate);
		transform.GetChild (0).GetComponent<Text> ().text = "Simulation";
	}
}
