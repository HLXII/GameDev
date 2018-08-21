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

		gameObject.GetComponent<Button> ().onClick.AddListener (Simulate);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Simulate() {
		Debug.Log ("Simulating");

        buildCanvas.StartSimulation ();

		gameObject.GetComponent<Button> ().onClick.RemoveAllListeners ();
		gameObject.GetComponent<Button> ().onClick.AddListener (EndSimulate);
		transform.GetChild (0).GetComponent<Text> ().text = "End Simulation";
	}

	void EndSimulate() {
		Debug.Log ("Ending Simulation");

        buildCanvas.EndSimulate ();

		gameObject.GetComponent<Button> ().onClick.RemoveAllListeners ();
		gameObject.GetComponent<Button> ().onClick.AddListener (CleanSimulation);
		transform.GetChild (0).GetComponent<Text> ().text = "Clean Simulation";
	}

	void CleanSimulation() {
		Debug.Log ("Cleaning Simulation");

		buildCanvas.CleanSimulation ();

		gameObject.GetComponent<Button> ().onClick.RemoveAllListeners ();
		gameObject.GetComponent<Button> ().onClick.AddListener (Simulate);
		transform.GetChild (0).GetComponent<Text> ().text = "Simulation";
	}
}
