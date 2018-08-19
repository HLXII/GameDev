using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rune", menuName = "Runes/Generic Rune", order = 1)]
public class GenericRuneTemplate : RuneTemplate
{

    public override void ManipulateEnergy(RuneData runeData)
    {
        throw new System.NotImplementedException();
    }

}