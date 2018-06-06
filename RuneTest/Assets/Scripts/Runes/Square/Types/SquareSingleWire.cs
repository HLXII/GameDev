using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareSingleWire : SquareRune {

	protected new void Start() {
		base.Start ();
		id += "Wire_Single_0";
		connections = new int[] { 0, 2 };
	}

	public override void manipulateEnergy ()
	{
		energyOut [1] = energyIn [0];
		energyOut [0] = energyIn [1];
	}


	/*
	public override void simulate ()
	{


		GameObject neighbor1 = neighbors [(connections [0] + rotation) % sides];
		GameObject neighbor2 = neighbors [(connections [1] + rotation) % sides];
		if (neighbor1 != null) {

			// This is the neighbor port direction connected
			int neighborPort = (connections [0] + rotation + 2) % sides;
			// Finding port index in neighbor
			int[] neighbor_connections = neighbor1.GetComponent<Rune>().Connections;
			for (int i = 0; i < neighbor_connections.Length; i++) {
				// Testing for true port direction
				if ((neighbor_connections[i] + neighbor1.GetComponent<Rune> ().Rotation) % sides == neighborPort) {
					next_energy [1] = (Energy)neighbor1.GetComponent<Rune> ().Energy [i];
					break;
				}
			}

		}

		if (neighbor2 != null) {

		}
	}*/
}
