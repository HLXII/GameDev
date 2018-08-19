using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rune ScriptableObject template
/// </summary>
public abstract class RuneTemplate : ScriptableObject
{

    // Type of rune (aka. how many sides)
    public enum RuneType { Triangle = 3, Square = 4, Hexagon = 6 };
    public RuneType runeType;

    // Id and Description
    public string id;
    public string description;

    // Interactable is usually dealing with the empty/void rune or other secret runes
    public bool isInteractable;
    // Movable allows for swapping/moving runes on the table
    public bool isMovable;

    public int[] connections;

    // Animator for the rune object
    public RuntimeAnimatorController animatorController;

    public abstract void ManipulateEnergy(RuneData runeData);

}

public class InputRuneTemplate : RuneTemplate
{
    public int inputRate;

    public override void ManipulateEnergy(RuneData runeData)
    {
        throw new System.NotImplementedException();
    }

}

public abstract class OutputRuneTemplate : RuneTemplate 
{
    public int maxRate;
    public int capacity;
    public int outputRate;

    public override void ManipulateEnergy(RuneData runeData)
    {
        throw new System.NotImplementedException();
    }

    public abstract void Release();

}


