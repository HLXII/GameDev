using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	void Update() {
		Camera.main.transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, -10);
	}

	// Update is called once per frame
	void FixedUpdate () {
		Vector2 force = new Vector2 (0, 0);
		if (Input.GetKey (KeyCode.W)) {
			force += new Vector2 (0, 1);
		}
		if (Input.GetKey (KeyCode.S)) {
			force += new Vector2 (0, -1);
		}
		if (Input.GetKey (KeyCode.A)) {
			force += new Vector2 (-1, 0);
		}
		if (Input.GetKey (KeyCode.D)) {
			force += new Vector2 (1, 0);
		}
        force = force.normalized * 5;
		gameObject.GetComponent<Rigidbody2D> ().velocity = force;

	}
}
