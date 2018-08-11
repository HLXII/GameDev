using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NatureRingData : ArmorData {

	public NatureRingData() {
		id = "Nature Ring";
		type = "High";
		description = "We call it a nature ring solely because it has a green gem.\nOriginal.";
	}

}

[System.Serializable]
public class TopHatData : ArmorData {

	public TopHatData() {
		id = "Top Hat";
		type = "High";
		description = "So dapper you'll be considered the dappiest!";
	}

}

[System.Serializable]
public class PinkBraData : ArmorData {

	public PinkBraData() {
		id = "Pink Bra";
		type = "Mid";
		description = "Why would you even think this would give you armor?";
	}

}

[System.Serializable]
public class TrashCanData : ArmorData {

	public TrashCanData() {
		id = "Trash Can";
		type = "Mid";
		description = "Now the outside matches the inside!";
	}

}

[System.Serializable]
public class SnakeBootData : ArmorData {

	public SnakeBootData() {
		id = "Snake Boot";
		type = "Low";
		description = "Did you steal this from Woody?";
	}

}

[System.Serializable]
public class HoverboardData : ArmorData {

	public HoverboardData() {
		id = "Hoverboard";
		type = "Low";
		description = "Now you can save minimal amounts of energy!";
	}

}
