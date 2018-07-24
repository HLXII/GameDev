using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;
using System;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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

[System.Serializable]
public class TableData {

	// Array of available runes in the table
	private List<RuneData> table;

	public TableData(string filename) {
		BinaryFormatter bf = new BinaryFormatter ();
		TextAsset dataFile = Resources.Load<TextAsset> (filename);
		Stream s = new MemoryStream (dataFile.bytes);
		TableData tableData = (TableData)bf.Deserialize (s);
		table = tableData.getTable ();
	}

	public TableData() {

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