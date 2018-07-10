using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;
using System;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class RuneComparer : IComparer<string> {

	int IComparer<string>.Compare(string rune1, string rune2) {
		string[] r1 = rune1.Split ('_');
		string[] r2 = rune2.Split ('_');

		if (r1 [0] [0] != r2 [0] [0]) {
			return r1 [0] [0] - r2 [0] [0];
		} else {
			return string.Compare(rune1,rune2);
		}
	}

}

[System.Serializable]
public class BuildData {

	// Array of available runes in the table
	private List<RuneData> table;

	// 2D Array of runes in the page
	private RuneData[,] page;

	private int[,] pageRotations;

	public BuildData() {
		int width = 3;
		int height = 3;

		table = new List<RuneData> ();

		table.Add (new SquareSingleWireData (2,3));
		table.Add (new SquareSingleWireData (3,4));
		table.Add (new SquareSingleWireData (5,6));
		table.Add (new SquareSingleWireData (6,7));
		table.Add (new SquareSingleWireData (7,8));
		table.Add (new SquareCrossData (1, 1));


		page = new RuneData[width, height];

		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				if (i % 2 == 0) {
					page [i, j] = new SquareSingleWireData (i, j);
				} else {
					page [i, j] = new EmptyData ();
				}
			}
		}

		pageRotations = new int[width, height];

		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				pageRotations [i, j] = (i + j) % 4;
			}
		}
	}

	public void addToTable(RuneData runeData) {
		//Debug.Log ("Adding " + runeString + " to table");
		table.Add (runeData);
	}

	public void removeFromTable(RuneData runeData) {
		table.Remove (runeData);
	}

	public List<RuneData> getTable() {
		List<RuneData> temp = new List<RuneData> (table);
		//temp.Sort ();
		return temp;
	}

	public List<RuneData> getTable(string classFilter, string rankFilter) {

		List<RuneData> temp = new List<RuneData> (table);
		//temp.Sort ();
		return temp;

		//Debug.Log ("Getting table with filters " + classFilter + " " + rankFilter);
		/*
		SortedList<string,int> intermediateList = new SortedList<string,int>(new RuneComparer());
		foreach (KeyValuePair<string,int> item in table) {
			//Debug.Log (item.Key);
			string[] data = item.Key.Split ('_');
			if (data[1].Contains(classFilter) && data[3].Contains(rankFilter)) {
				intermediateList.Add (item.Key, item.Value);
			}
		}

		return intermediateList;*/
	}

	public RuneData[,] getPage() {
		return page;
	}

	public int[,] getPageRotation() {
		return pageRotations;
	}

}