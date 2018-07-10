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

	public BuildData() {
		int width = 3;
		int height = 3;

		table = new List<RuneData> ();

		table.Add (new SquareSingleWireData ());
		table.Add (new SquareSingleWireData ());
		table.Add (new SquareSingleWireData ());
		table.Add (new SquareSingleWireData ());
		table.Add (new SquareSingleWireData ());


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
	}

	/*
	// Generator used to make puzzle instances by code, rather than by hand
	public BuildData() {

		// Size of board.
		int width = 10;
		int height = 2;

		table = new SortedList<string,int> (new RuneComparer());

		table.Add ("S_Output_Sink_0", 100);
		table.Add ("S_Input_Source_0", 100);
		table.Add ("S_Wire_TJunction_0", 100);
		table.Add("S_Wire_FourWay_0",100);
		table.Add ("S_Wire_Cross_0", 100);
		table.Add ("S_Wire_Single_0", 100);

		page = new string[width, height];
		//this.page = new string[,] {{"SX","S_","S_"},{"SX","SX","SX"},{"S_","SX","S_"}};
		//page = new string[,] {
		//	{ "S_Special_Void_0", "S_Special_Void_0", "S_Input_Source_0", "S_Special_Block_0" },
		//	{ "S_Special_Empty_0", "S_Special_Empty_0", "S_Wire_Single_0", "S_Special_Empty_0" },
		//	{ "S_Special_Void_0", "S_Special_Empty_0", "S_Special_Empty_0", "S_Special_Empty_0" },
		//	{ "S_Special_Block_0", "S_Special_Block_0", "S_Special_Empty_0", "S_Special_Block_0" }
		//};

		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				this.page [i, j] = "S_Special_Empty_0";
			}
		}

	}*/ 

	public void swapOnPage(int x1, int y1, int x2, int y2) {
		RuneData temp = page [x1, y1];
		page [x1, y1] = page [x2, y2];
		page [x2, y2] = temp;
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

}