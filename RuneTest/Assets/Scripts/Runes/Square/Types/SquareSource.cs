using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareSource : SquareRune {

	protected new void Start() {
		base.Start ();
		id += "Input_Source_0";
		connections = new int[] { 0 };
		energyIn = new Energy[1];
		energyOut = new Energy[1];
	}

	public override void manipulateEnergy ()
	{
		if (energyIn [0] == null) {
			energyOut [0] = new Energy(10);
		}
	}

	/*
	public override void simulate ()
	{
		int outPort = (connections [0] + rotation) % sides;
		GameObject outNeighbor = neighbors [outPort];
		// Neighbor exists on the side of the outport
		if (outNeighbor != null) {
			// Neighbor is connected to the outport
			int neighborInPort = (outPort + (sides)/2)% sides;
			if (outNeighbor.GetComponent<Rune>().isConnected(neighborInPort) {
				// Send energy to neighbor's port
				outNeighbor.GetComponent<Rune>().sendEnergy(new Energy(10), neighborInPort);
			}
		}
	}*/


}
