using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy {

	private int power;

	public Energy(int power) {
		this.power = power;
	}

	public int Power { get { return power; } set { power = value; } }

	public override string ToString() {
		string o = "";
		o += power.ToString ();
		return o;
	}

}

/// <summary>
/// Rune data to be stored in files and used to initialize the rune GameObjects
/// </summary>
[System.Serializable]
public class RuneData {

	protected string id;

	public RuneData() {
	}

	public string Id {get{return id;} }

	public override string ToString() {
		return id;
	}

}

[System.Serializable]
public class EmptyData : RuneData {
	public EmptyData() {
		id = "Empty";
	}
}
[System.Serializable]
public class BlockData : RuneData {
	public BlockData() {
		id = "Block";
	}
}
[System.Serializable]
public class VoidData : RuneData {
	public VoidData() {
		id = "Void";
	}
}
[System.Serializable]
public class WireData : RuneData {

	protected int loss;
	protected int capacity;
	public int Loss { get { return loss; } }
	public int Capacity { get { return capacity; } }

	public WireData(int loss, int capacity) {
		this.loss = loss;
		this.capacity = capacity;
	}

	public override string ToString () {
		string o = "";
		o += id + " ";
		o += "Loss: " + loss + " ";
		o += "Capacity: " + capacity;
		return o;
	}

}

[System.Serializable]
public class InputData : RuneData {

	protected int inputRate;
	public int InputRate { get { return inputRate; } }

	public InputData(int inputRate) {
		this.inputRate = inputRate;
	}

	public override string ToString () {
		string o = "";
		o += id + " ";
		o += "Input Rate: " + inputRate;
		return o;
	}

}

[System.Serializable]
public class OutputData : RuneData {

	protected int maxRate;
	protected int capacity;
	protected int outputRate;
	public int MaxRate { get { return maxRate; } }
	public int Capacity { get { return capacity; } }
	public int OutputRate{ get { return outputRate; } }

	public OutputData(int maxRate, int capacity, int outputRate) {
		this.maxRate = maxRate;
		this.capacity = capacity;
		this.outputRate = outputRate;
	}

	public override string ToString () {
		string o = "";
		o += id + " ";
		o += "Max Rate: " + maxRate + " ";
		o += "Capacity: " + capacity + " ";
		o += "Output Rate: " + outputRate + " ";
		return o;
	}

}
