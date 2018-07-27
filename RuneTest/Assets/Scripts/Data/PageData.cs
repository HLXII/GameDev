using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;
using System;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

// Class for storing rune pages
[System.Serializable]
public class PageData {

	// 2D Array of runes in the page
	private RuneData[,] page;

	// Current rotation of the runes
	private int[,] pageRotations;

	public RuneData[,] Page { get { return page; } }
	public int[,] PageRotations { get { return pageRotations; } }

	// Constructing from file (usually initializing puzzles)
	public PageData(string filename) {
		BinaryFormatter bf = new BinaryFormatter ();
		TextAsset dataFile = Resources.Load<TextAsset> (filename);
		Stream s = new MemoryStream (dataFile.bytes);
		PageData pageData = (PageData)bf.Deserialize (s);
		page = pageData.Page;
		pageRotations = pageData.PageRotations;
	}

	// Initializing for test
	public PageData() {

		System.Random random = new System.Random ();

		int height = 5;
		int width = 7;

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

}