using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

[System.Serializable]
public class Energy
{

    [SerializeField]
    private int power;
    public int Power { get { return power; } set { power = value; } }

    public enum EnergyAttribute { Neutral, Fire, Water, Earth, Air };
    [SerializeField]
    private EnergyAttribute attribute;
    public EnergyAttribute Attribute { get { return attribute; } set { attribute = value; } }

    public Energy(int power)
    {
        this.attribute = EnergyAttribute.Neutral;
        this.power = power;
    }

    public Energy(EnergyAttribute attribute, int power)
    {
        this.attribute = attribute;
        this.power = power;
    }

    public Energy(Energy energy)
    {
        this.attribute = energy.Attribute;
        this.power = energy.power;
    }

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
    public int Rank { get { return rank1 + rank2 + rank3; } }


    public RuneData(string runeTemplateId)
    {
        this.runeTemplateId = runeTemplateId;

        runeTemplate = DataManager.rune[runeTemplateId];

        rank1 = 0;
        rank2 = 0;
        rank3 = 0;
    }

    public RuneData(string runeTemplateId, int rank1, int rank2, int rank3)
    {
        this.runeTemplateId = runeTemplateId;
        runeTemplate = DataManager.rune[runeTemplateId];

        this.rank1 = rank1;
        this.rank2 = rank2;
        this.rank3 = rank3;
    }

}