using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

public class Energy
{

    private int power;

    public Energy(int power)
    {
        this.power = power;
    }

    public int Power { get { return power; } set { power = value; } }

    public override string ToString()
    {
        string o = "";
        o += power.ToString();
        return o;
    }

}

/// <summary>
/// Rune data to be stored in files and used to initialize the rune GameObjects
/// </summary>
[System.Serializable]
public class RuneData
{

    private readonly string runeTemplateId;
    public string RuneTemplateId { get { return runeTemplateId; } }

    [IgnoreDataMember]
    [System.NonSerialized]
    RuneTemplate runeTemplate;
    public RuneTemplate RuneTemplate
    {
        get
        {
            if (runeTemplate == null)
            {
                runeTemplate = DataManager.rune[RuneTemplateId];
            }
            return runeTemplate;
        }
    }

    private int rank1;
    public int Rank1 { get { return rank1; } set { rank1 = value; } }
    private int rank2;
    public int Rank2 { get { return rank2; } set { rank2 = value; } }
    private int rank3;
    public int Rank3 { get { return rank3; } set { rank3 = value; } }

    [IgnoreDataMember]
    [System.NonSerialized]
    RuneData[] neighbors;
    public RuneData[] Neighbors { get { return neighbors; } set { neighbors = value; } }

    private Energy[] energyIn;
    public Energy[] EnergyIn { get { return energyIn; } set { energyIn = value; } }
    private Energy[] energyOut;
    public Energy[] EnergyOut { get { return energyOut; } set { energyOut = value; } }

    public RuneData(string runeTemplateId)
    {
        this.runeTemplateId = runeTemplateId;

        runeTemplate = DataManager.rune[runeTemplateId];

        neighbors = new RuneData[(int)runeTemplate.runeType];
        for (int i = 0; i < neighbors.Length;i++)
        {
            neighbors[i] = null;
        }
        energyIn = new Energy[runeTemplate.connections.Length];
        energyOut = new Energy[runeTemplate.connections.Length];

        rank1 = 0;
        rank2 = 0;
        rank3 = 0;
    }

    public RuneData(string runeTemplateId, int rank1, int rank2, int rank3)
    {
        this.runeTemplateId = runeTemplateId;
        runeTemplate = DataManager.rune[runeTemplateId];

        neighbors = new RuneData[(int)runeTemplate.runeType];
        energyIn = new Energy[runeTemplate.connections.Length];
        energyOut = new Energy[runeTemplate.connections.Length];

        this.rank1 = rank1;
        this.rank2 = rank2;
        this.rank3 = rank3;
    }

}
