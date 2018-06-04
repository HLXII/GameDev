using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;
using System;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class RuneByClass : IComparer<string> {

	int IComparer<string>.Compare(string rune1, string rune2) {
		string[] r1 = rune1.Split ('_');
		string[] r2 = rune2.Split ('_');
		return string.Compare(rune1,rune2);
	}

}

[System.Serializable]
public class BuildData {

	// Dictionary of all the runes available in the table
	// Keys are runeStrings, while Values are the number of available runes
	private Dictionary<string,int> table;

	// 2D Array of runeStrings of the runes in the page
	private string[,] page;

	// Generator used to make puzzle instances by code, rather than by hand
	public BuildData() {

		// Size of board.
		int width = 4;
		int height = 4;

		table = new Dictionary<string,int> ();

		table.Add ("S_Wire_Single_0", 2);
		table.Add ("S_Output_Sink_0", 1);

		page = new string[width, height];
		//this.page = new string[,] {{"SX","S_","S_"},{"SX","SX","SX"},{"S_","SX","S_"}};
		page = new string[,] {
			{ "S_Special_Void_0", "S_Special_Void_0", "S_Input_Source_0", "S_Special_Block_0" },
			{ "S_Special_Empty_0", "S_Special_Empty_0", "S_Wire_Single_0", "S_Special_Empty_0" },
			{ "S_Special_Void_0", "S_Special_Empty_0", "S_Special_Empty_0", "S_Special_Empty_0" },
			{ "S_Special_Block_0", "S_Special_Block_0", "S_Special_Empty_0", "S_Special_Block_0" }
		};
		/*
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				if (j != 0 && j != height-1) {
					this.page [i,j] = "SX";
				} else {
					this.page [i,j] = "S_";
				}
			}
		}*/

	}

	public SortedList<string,int> getTable(IComparer<string> compare) {
		return new SortedList<string,int>(this.table,new RuneByClass());
	}

	public SortedList<string,int> getTable(IComparer<string> compare, string[,] filters) {
		SortedList<string,int> intermediateList = new SortedList<string,int> (table, compare);
		return null;
	}

	public string[,] getPage() {
		return page;
	}

}