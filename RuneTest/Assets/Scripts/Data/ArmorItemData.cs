using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HighArmorData : ArmorData {

}

[System.Serializable]
public class MidArmorData : ArmorData {

}


[System.Serializable]
public class LowArmorData : ArmorData {

}

[System.Serializable]
public class NatureRingData : HighArmorData {

	public NatureRingData() {
		id = "Nature Ring";
		description = "We call it a nature ring solely because it has a green gem.\nOriginal.";
	}

}

[System.Serializable]
public class TopHatData : HighArmorData {

	public TopHatData() {
		id = "Top Hat";
		description = "So dapper you'll be considered the dappiest!";
	}

}

[System.Serializable]
public class PinkBraData : MidArmorData {

	public PinkBraData() {
		id = "Pink Bra";
		description = "Why would you even think this would give you armor?";
	}

}

[System.Serializable]
public class TrashCanData : MidArmorData {

	public TrashCanData() {
		id = "Trash Can";
		description = "Now the outside matches the inside!";
	}

}

[System.Serializable]
public class SnakeBootData : LowArmorData {

	public SnakeBootData() {
		id = "Snake Boot";
		description = "Did you steal this from Woody?";
	}

}

[System.Serializable]
public class HoverboardData : LowArmorData {

	public HoverboardData() {
		id = "Hoverboard";
		description = "Now you can save minimal amounts of energy!";
	}

}
