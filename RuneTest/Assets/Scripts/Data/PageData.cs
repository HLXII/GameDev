using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;
using System;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class PageData {

	// 2D Array of runes in the page
	private RuneData[,] page;

	private int[,] pageRotations;

	public RuneData[,] Page { get { return page; } }
	public int[,] PageRotations { get { return pageRotations; } }

	public PageData(string filename) {
		BinaryFormatter bf = new BinaryFormatter ();
		TextAsset dataFile = Resources.Load<TextAsset> (filename);
		Stream s = new MemoryStream (dataFile.bytes);
		PageData pageData = (PageData)bf.Deserialize (s);
		page = pageData.Page;
		pageRotations = pageData.PageRotations;
	}

	public PageData() {

	}

}