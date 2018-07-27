using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;
using System;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


// Old Class for storing build data
[System.Serializable]
public class BuildData {

	// Array of available runes in the table
	private List<RuneData> table;

	// 2D Array of runes in the page
	private RuneData[,] page;

	private int[,] pageRotations;

	public BuildData(int width, int height) {

		System.Random random = new System.Random ();

		table = new List<RuneData> ();

		table.Add (new SquareSingleWireData(0,20));
		table.Add (new SquareSingleWireData (5,30));
		table.Add (new SquareCrossData (3, 20));
		table.Add (new SquareSourceData (20));
		table.Add (new SquareSourceData (10));
		table.Add (new SquareCornerData (10, 20));
		table.Add (new SquareSinkData (10, 100, 5));
		table.Add (new SquareSinkData (20, 100, 10));

		page = new RuneData[height, width];
		pageRotations = new int[height, width];

		int[,] generation = new int[height, width];

		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				generation [i, j] = random.Next () % 3;
			}
		}

		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				if (generation [i, j] == 0) {
					page [i, j] = new VoidData ();
				} else {
					page [i, j] = new EmptyData ();
				}
				pageRotations [i, j] = 0;
			}
		}

	}


	public BuildData() {
		int width = 20;
		int height = 20;

		table = new List<RuneData> ();

		table.Add (new SquareSingleWireData (2,3));
		table.Add (new SquareSingleWireData (3,4));
		table.Add (new SquareSingleWireData (5,6));
		table.Add (new SquareSingleWireData (6,7));
		table.Add (new SquareSingleWireData (7,8));
		table.Add (new SquareCrossData (1, 1));
		table.Add (new SquareSourceData (1));


		page = new RuneData[height, width];

		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				if (i % 3 == 0) {
					page [i, j] = new SquareSingleWireData (i, j);
				} else if (i%3 == 1) {
					page [i, j] = new EmptyData ();
				} else {
					page [i,j] = new VoidData();
				}
			}
		}

		pageRotations = new int[height, width];

		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
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