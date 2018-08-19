//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;

//[System.Serializable]
//public class SquareSourceData : InputData {

//	public SquareSourceData(int inputRate) : base(inputRate) {
//		id = "Source";
//	}

//}

//public class SquareSource : SquareRune {

//	protected new void Start() {
//		base.Start ();
//		numConnections = 1;
//		connections = new int[] { 0 };
//		initEnergy ();
//	}

//	public override void reset ()
//	{
//		base.reset ();
//		gameObject.GetComponent<Animator> ().Play ("off");
//		gameObject.GetComponent<Animator> ().SetBool ("on", false);
//	}

//	public override void manipulateEnergy ()
//	{
//		// If no backwards flowing energy
//		if (energyIn [0] == null) {
//			energyOut [0] = new Energy (((InputData)runeData).InputRate);
//			gameObject.GetComponent<Animator> ().SetBool ("on", true);
//			// Backflowing energy
//		} else {
//			signalReciever.receiveSignal ("Source receiving energy");
//			gameObject.GetComponent<Animator> ().SetTrigger ("error");
//		}
//	}
		
//	public override string getInfo ()
//	{
//		string o = "";
//		o += runeData.ToString();

//		return o;

//	}
//}
