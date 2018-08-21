using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rune", menuName = "Runes/Generic Rune", order = 1)]
public class GenericRuneTemplate : RuneTemplate
{

    public override string GetInfo(RuneSlot runeSlot)
    {
        return id + '\n' + description;
    }

    public override void ManipulateEnergy(RuneSlot runeSlot, BuildSignalManager buildSignalManager)
    {
        return;
    }

}