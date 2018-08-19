using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rune", menuName = "Runes/Transfer/Generic Transfer Rune", order = 1)]
public class TransferRuneTemplate : RuneTemplate
{

    public int[] lossScale;
    public int[] capacityScale;

    public override void ManipulateEnergy(RuneData runeData)
    {
        throw new System.NotImplementedException();
    }

}