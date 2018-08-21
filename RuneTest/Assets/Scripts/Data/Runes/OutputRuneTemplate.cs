using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OutputRuneTemplate : RuneTemplate
{

    public int[] maxRateScale;
    public int[] capacityScale;
    public int[] outputRateScale;

    public override string GetInfo(RuneSlot runeSlot)
    {
        return id + '\n' +
            "Max Rate: " + maxRateScale[runeSlot.RuneData.Rank1] + '\n' +
            "Capacity: " + runeSlot.Storage + "/" + capacityScale[runeSlot.RuneData.Rank2] + '\n' + 
            "Output Rate: " + outputRateScale[runeSlot.RuneData.Rank3];
    }

    public override void ManipulateEnergy(RuneSlot runeSlot, BuildSignalManager buildSignalManager)
    {
    //    Releasing stored energy
    //    storage = Mathf.Max(0, storage - ((SquareSinkData)runeData).OutputRate);

    //    if (energyIn [0] != null) {
    //    // If input energy is less than max rate
    //    if (energyIn [0].Power <= ((SquareSinkData)runeData).MaxRate) {
    //        // Store inputted energy into storage
    //        storage += energyIn [0].Power;

    //        // If stored energy is greater than max capacity
    //        if (storage > ((SquareSinkData)runeData).Capacity) {
    //              signalReciever.receiveSignal ("Sink over max capacity");
    //              gameObject.GetComponent<Animator> ().SetTrigger ("error");
    //        }
    //          // Input over max rate
    //    } else {
    //          signalReciever.receiveSignal ("Sink receiving over max rate");
    //          gameObject.GetComponent<Animator> ().SetTrigger ("error");
    //    }
    //}

    }

    public abstract void Release();

}