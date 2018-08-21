using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rune ScriptableObject template
/// </summary>
public abstract class RuneTemplate : ScriptableObject
{

    // Type of rune
    public int sides;

    // Id and Description
    public string id;
    public string description;

    // Interactable is usually dealing with the empty/void rune or other secret runes
    public bool isInteractable;
    // Movable allows for swapping/moving runes on the table
    public bool isMovable;

    public int[] connections;

    // SpriteSheet for different rotations
    public Sprite[] runeCovers;

    // Animator for the rune object
    public RuntimeAnimatorController animatorController;

    public abstract void ManipulateEnergy(RuneSlot runeSlot, BuildSignalManager buildSignalManager);

    public string GetInfo()
    {
        return id + '\n' + description;
    }

    public abstract string GetInfo(RuneSlot runeSlot);
}




