using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;
using System;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

// IComparer for sorting runes
public class GenericRuneComparer : IComparer<RuneData> {

	int IComparer<RuneData>.Compare(RuneData rune1, RuneData rune2) {
        if (rune1.RuneTemplate.GetType() == rune2.RuneTemplate.GetType())
        {
            if (rune1.Rank == rune2.Rank)
            {
                int weightedRank1 = rune1.Rank1 * 25 + rune1.Rank2 * 5 + rune1.Rank3;
                int weightedRank2 = rune2.Rank1 * 25 + rune2.Rank2 * 5 + rune2.Rank3;
                return weightedRank2 - weightedRank1;
            }
            else
            {
                return rune2.Rank - rune1.Rank;
            }


        } else {
            return string.Compare(rune2.RuneTemplate.GetType().Name, rune1.RuneTemplate.GetType().Name);
        }
	}

}

// Class for storing runes in the table during builds
[System.Serializable]
public class TableData {

	// Array of available runes in the table
	private List<RuneData> table;

	public List<RuneData> Table { get { return table; } }

	// Constructing from file (Usually for initializing puzzles)
	public TableData(string filename) {
		BinaryFormatter bf = new BinaryFormatter ();
		TextAsset dataFile = Resources.Load<TextAsset> (filename);
		Stream s = new MemoryStream (dataFile.bytes);
		TableData tableData = (TableData)bf.Deserialize (s);
		table = tableData.getTable ();
	}

	// Initializing from list of runes (Idk if will be used if we can directly edit the inventory runes)
	public TableData(List<RuneData> runes) {
		table = runes;
	}

	// Initializing for testing
	public TableData() {

		table = new List<RuneData> ();

	}

	public List<RuneData> getTable() {
		return table;
	}

	public List<RuneData> getTable(string classFilter) {
		List<RuneData> filteredTable;

		switch (classFilter) {
		case "Wire":
			filteredTable = table.FindAll (FindWire);
			break;
		case "Input":
			filteredTable = table.FindAll (FindInput);
			break;
		case "Output":
			filteredTable = table.FindAll (FindOutput);
			break;
		default:
			filteredTable = table;
			break;
		}
		filteredTable.Sort (new GenericRuneComparer());
		return filteredTable;
	}

	private static bool FindWire(RuneData rune)
	{
        return rune.RuneTemplate is WireRuneTemplate;
	}
	private static bool FindInput(RuneData rune)
	{
        return rune.RuneTemplate is InputRuneTemplate;
    }
	private static bool FindOutput(RuneData rune)
	{
        return rune.RuneTemplate is OutputRuneTemplate;
    }

}