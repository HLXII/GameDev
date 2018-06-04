using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class LoadSceneOnClick : MonoBehaviour {

	public void LoadScene(int index) {
		SceneManager.LoadScene (index);
	}

}
