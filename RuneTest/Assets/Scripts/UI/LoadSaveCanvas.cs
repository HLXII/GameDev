using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadSaveCanvas : MonoBehaviour {

	public GameObject savePanel;

	public GameObject saveContent;

	private DataManager dm;

	// Use this for initialization
	void Start () {
		dm = GameObject.Find ("DataManager").GetComponent<DataManager> ();

		//dm.createNewSave ();
		//dm.save ("save1.sav");

		loadSaves ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void loadSaves() {

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
		} else {
			foreach (string save in saves) {
				Debug.Log (save);

				GameObject instance = Instantiate (savePanel, saveContent.transform);

				instance.transform.GetChild (1).GetComponent<Text> ().text = save;

				instance.GetComponent<Button> ().onClick.AddListener (delegate {
					dm.load (save);
					SceneManager.LoadScene("OverWorld");
				});

			}
		}


	}

	public void loadScene(string scene) {
		SceneManager.LoadScene (scene);
	}
}
