using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rune", menuName = "Runes/Transfer/Wires/Generic Wire Rune", order = 1)]
public class WireRuneTemplate : TransferRuneTemplate
{

    public override void ManipulateEnergy(RuneSlot runeSlot, BuildSignalManager buildSignalManager)
    {

        RuneData runeData = runeSlot.RuneData;

        Energy[] energyIn = runeSlot.EnergyIn;
        Energy[] energyOut = runeSlot.EnergyOut;

        int loss = lossScale[runeData.Rank1];
        int capacity = capacityScale[runeData.Rank2];

        if (energyIn[0] != null && energyIn[1] != null)
        {
            Debug.Log("Wire receiving energy from both ports");
            buildSignalManager.receiveSignal ("Wire receiving energy from both ports");
            //gameObject.GetComponent<Animator> ().SetTrigger ("error");
        }
        else
        {
            if (energyIn[0] != null)
            {
                if (energyIn[0].Power > capacity)
                {
                    Debug.Log("Wire over max capacity");
                    buildSignalManager.receiveSignal ("Wire over max capacity");
                    //gameObject.GetComponent<Animator> ().SetTrigger ("error");
                }
                else
                {
                    energyIn[0].Power = (int) (energyIn[0].Power * ((100-loss) / 100.0F));
                    if (energyIn[0].Power <= 0)
                    {
                        energyOut[1] = null;
                    }
                    else
                    {
                        energyOut[1] = energyIn[0];
                    }
                    //gameObject.GetComponent<Animator> ().SetBool ("outputting", true);
                }
            }
            else if (energyIn[1] != null)
            {
                if (energyIn[1].Power > capacity)
                {
                    Debug.Log("Wire over max capacity");
                    buildSignalManager.receiveSignal ("Wire over max capacity");
                    //gameObject.GetComponent<Animator> ().SetTrigger ("error");
                }
                else
                {
                    energyIn[1].Power = (int)(energyIn[1].Power * ((100 - loss) / 100.0F));
                    if (energyIn[1].Power <= 0)
                    {
                        energyOut[0] = null;
                    }
                    else
                    {
                        energyOut[0] = energyIn[1];
                    }
                    //gameObject.GetComponent<Animator> ().SetBool ("outputting", true);
                }
            }
            else
            {
                //gameObject.GetComponent<Animator> ().SetBool ("outputting", false);
            }
        }

        for (int i = 0; i < energyIn.Length; i++)
        {
            energyIn[i] = null;
        }

    }

}