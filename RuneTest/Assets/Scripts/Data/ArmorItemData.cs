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
