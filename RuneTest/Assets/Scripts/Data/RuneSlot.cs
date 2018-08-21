using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization;

[System.Serializable]
public class RuneSlot
{

    private RuneData runeData;
    public RuneData RuneData 
    { 
        get { return runeData; } 
        set 
        {
            if (runeData == value) return;
            runeData = value;
            if (OnRuneDataChange != null)
                OnRuneDataChange(runeData);
        } 
    }
    public delegate void OnRuneDataChangeDelegate(RuneData runeData);
    public event OnRuneDataChangeDelegate OnRuneDataChange;

    private int rotation;
    public int Rotation 
    { 
        get { return rotation; } 
        set 
        { 
            if (rotation == value) return;
            rotation = value;
            if (OnRotationChange != null)
                OnRotationChange(rotation);
        } 
    }
    public delegate void OnRotationChangeDelegate(int rotation);
    public event OnRotationChangeDelegate OnRotationChange;

    private bool active;
    public bool Active { get { return active; } set { active = value; } }

    private int storage;
    public int Storage { get { return storage; } set { storage = value; } }

    [IgnoreDataMember]
    [System.NonSerialized]
    RuneSlot[] neighbors;
    public RuneSlot[] Neighbors { get { return neighbors; } set { neighbors = value; } }

    private Energy[] energyIn;
    public Energy[] EnergyIn { get { return energyIn; } set { energyIn = value; } }
    private Energy[] energyOut;
    public Energy[] EnergyOut { get { return energyOut; } set { energyOut = value; } }

    public RuneSlot(RuneData runeData)
    {
        this.runeData = runeData;
        rotation = 0;

        neighbors = new RuneSlot[(int)runeData.RuneTemplate.sides];
        energyIn = new Energy[runeData.RuneTemplate.connections.Length];
        energyOut = new Energy[runeData.RuneTemplate.connections.Length];

        active = false;
        storage = 0;
    }

    public RuneSlot(RuneData runeData, int rotation)
    {
        this.runeData = runeData;
        this.rotation = rotation;

        neighbors = new RuneSlot[(int)runeData.RuneTemplate.sides];
        energyIn = new Energy[runeData.RuneTemplate.connections.Length];
        energyOut = new Energy[runeData.RuneTemplate.connections.Length];

        active = false;
        storage = 0;
    }

    private bool PortOpen(int direction)
    {
        foreach(int connection in runeData.RuneTemplate.connections)
        {
            int truePort = (connection + rotation) % (int)runeData.RuneTemplate.sides;
            if (direction == truePort) {
                return true;
            }
        }
        return false;
    }

    public void Reset() 
    {
        active = false;
        storage = 0;

        for (int i = 0; i < energyIn.Length;i++)
        {
            energyIn[i] = null;
            energyOut[i] = null;
        }
    }

    public void FindNeighbors(RuneSlot[,] runeSlots)
    {
        Debug.Log("Finding Neighbors for " + runeData.RuneTemplateId);

        if (neighbors == null)
        {
            neighbors = new RuneSlot[(int)runeData.RuneTemplate.sides];
        }

        int page_h = runeSlots.GetLength(0);
        int page_w = runeSlots.GetLength(1);

        int rune_x = -1;
        int rune_y = -1;

        for (int h = 0; h < page_h; h++)
        {
            for (int w = 0; w < page_w; w++)
            {
                if (runeSlots[h, w] == this)
                {
                    rune_x = w;
                    rune_y = h;
                }
            }
        }

        if (rune_x == -1 || rune_y == -1)
        {
            Debug.Log("Error: RuneData not in Page.");
            return;
        }

        Debug.Log("Current Index : " + rune_y + " " + rune_x);

        switch (runeData.RuneTemplate.sides)
        {
            case 4:

                Vector2[] possibleNeighbors = {
                    new Vector2(rune_x+1,rune_y),
                    new Vector2(rune_x,rune_y-1),
                    new Vector2(rune_x-1,rune_y),
                    new Vector2(rune_x,rune_y+1)
                };

                for (int i = 0; i < 4; i++)
                {

                    Vector2 possibleNeighbor = possibleNeighbors[i];
                    int n_x = (int)possibleNeighbor.x;
                    int n_y = (int)possibleNeighbor.y;

                    if (n_x < 0 || n_x >= page_w || n_y < 0 || n_y >= page_h)
                    {
                        neighbors[i] = null;
                    }
                    else
                    {
                        Debug.Log(n_y + " " + n_x);
                        RuneSlot neighbor = runeSlots[n_y, n_x];

                        int neighborConnectionPort = (i + 2) % 4;

                        neighbors[i] = neighbor.PortOpen(neighborConnectionPort) ? neighbor : null;

                    }
                }
                break;
            case 6:
                break;
            case 3:
                break;
        }

    }

    public void SendEnergy(BuildSignalManager buildSignalManager)
    {
        // Looping through all energy output
        for (int i = 0; i < energyOut.Length; i++)
        {
            // If there is energy to be outputted
            if (energyOut[i] != null)
            {
                int outPort = (runeData.RuneTemplate.connections[i] + rotation) % (int)runeData.RuneTemplate.sides;
                int neighborInPort = (outPort + (int)runeData.RuneTemplate.sides / 2) % (int)runeData.RuneTemplate.sides;
                // If there is a neighbor to output to
                if (neighbors[outPort] != null)
                {
                    RuneSlot neighbor = neighbors[outPort];

                    neighbor.ReceiveEnergy(energyOut[i], neighborInPort);

                }
                else
                {
                    // No neighbor, energy leak

                    buildSignalManager.receiveSignal("Energy leak");
                    //gameObject.GetComponent<Animator>().SetTrigger("error");
                }

                // Clearing energyOut
                energyOut[i] = null;
            }

        }
    }

    public void ReceiveEnergy(Energy energyIn, int port)
    {
        for (int i = 0; i < runeData.RuneTemplate.connections.Length; i++) {
            if ((runeData.RuneTemplate.connections [i] + rotation) % (int)runeData.RuneTemplate.sides == port) {
              this.energyIn [i] = energyIn;
              break;
          }
        }
    }

    public string GetInfo()
    {

        return active ? runeData.RuneTemplate.GetInfo(this) : runeData.RuneTemplate.GetInfo();
    }

}