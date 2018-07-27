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
		if (rune1.GetType() == rune2.GetType()) {
			switch (rune1.GetType().Name) {
			case "WireData":
				break;
			case "InputData":
				break;
			case "OutputData":
				break;
			default:
				break;
			}
			return -1;
		} else {
			return string.Compare (rune1.Id, rune2.Id);
		}
	}

}

// Class for storing runes in the table during builds
[System.Serializable]
public class TableData {

	// Array of available runes in the table
	private List<RuneData> table;

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

		table.Add (new SquareSingleWireData(0,20));
		table.Add (new SquareSingleWireData (5,30));
		table.Add (new SquareCrossData (3, 20));
		table.Add (new SquareSourceData (20));
		table.Add (new SquareSourceData (10));
		table.Add (new SquareCornerData (10, 20));
		table.Add (new SquareSinkData (10, 100, 5));
		table.Add (new SquareSinkData (20, 100, 10));

	}

	public List<RuneData> getTable() {
		return table;
	}

	public void addToTable(RuneData runeData) {
		table.Add (runeData);
	}

	public void removeFromTable(RuneData runeData) {
		table.Remove (runeData);
	}

}