using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCanvas : MonoBehaviour {

	public GameObject AirBlock;
	public GameObject StoneBlock;

	private DataManager dm;

	private static Dictionary<string,GameObject> blockDict; 

	// Use this for initialization
	void Start () {

		initDict ();

		dm = GameObject.Find ("DataManager").GetComponent<DataManager> ();

		BoardData boardData = dm.LoadBoard ();

		RectTransform field = (RectTransform)transform.GetChild (0);


		RectTransform t = (RectTransform)transform;


		t.sizeDelta = new Vector2 (boardData.Board.GetLength (1) * 10, boardData.Board.GetLength (0) * 10);

		for (int h = 0;h < boardData.Board.GetLength(0);h++) {
			for (int w = 0;w < boardData.Board.GetLength(1);w++) {
				Instantiate (blockDict [boardData.Board [h, w]], field);
			}
		}
	}

	public void initDict() {
		blockDict = new Dictionary<string,GameObject>() 
		{
			{"Air", AirBlock},
			{"Stone",StoneBlock}
		};
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
