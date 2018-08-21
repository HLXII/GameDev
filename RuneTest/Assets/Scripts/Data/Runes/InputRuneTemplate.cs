using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rune", menuName = "Runes/Input/Generic Input Rune", order = 1)]
public class InputRuneTemplate : RuneTemplate
{

    public Energy[] energyScale;

    public override string GetInfo(RuneSlot runeSlot)
    {
        return id + '\n' + "Energy Input: " + energyScale[runeSlot.RuneData.Rank1];
    }

    public override void ManipulateEnergy(RuneSlot runeSlot, BuildSignalManager buildSignalManager)
    {

        // Rune is currently sending energy
        if (runeSlot.Active)
        {

            //If no backwards flowing energy
            if (runeSlot.EnergyIn[0] == null)
            {
                runeSlot.EnergyOut[0] = new Energy(energyScale[runeSlot.RuneData.Rank1]);
                //gameObject.GetComponent<Animator>().SetBool("on", true);

            }
            else
            {
                // Backflowing energy, send error
                buildSignalManager.receiveSignal("Source receiving energy");
                //gameObject.GetComponent<Animator>().SetTrigger("error");
            }

        }

       
    }

}