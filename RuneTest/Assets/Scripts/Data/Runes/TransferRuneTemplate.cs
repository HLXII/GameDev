using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rune", menuName = "Runes/Transfer/Generic Transfer Rune", order = 1)]
public class TransferRuneTemplate : RuneTemplate
{

    public int[] lossScale;
    public int[] capacityScale;

    public override string GetInfo(RuneSlot runeSlot)
    {
        int maxFlow = 0;
        foreach(Energy energyOut in runeSlot.EnergyOut)
        {
            if (energyOut != null && energyOut.Power > maxFlow)
                maxFlow = energyOut.Power;
        }

        return id + '\n' +
            "Loss: " + lossScale[runeSlot.RuneData.Rank1] + '\n' +
            "Capacity: " + maxFlow + "/" + capacityScale[runeSlot.RuneData.Rank2];
    }

    public override void ManipulateEnergy(RuneSlot runeSlot, BuildSignalManager buildSignalManager)
    {
        return;
    }

}