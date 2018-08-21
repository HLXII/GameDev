using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;
using System;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;

// Class for storing rune pages
[System.Serializable]
public class PageData {

	// Type of rune page
	private readonly int sides;
	public int Sides { get { return sides; } }

    // 2D Array of RuneSlots
    private readonly RuneSlot[,] page;
    public RuneSlot[,] Page { get { return page; } }

    private bool active;
    public bool Active
    {
        get { return active; }
        set
        {
            active = value;
            for (int h = 0; h < page.GetLength(0); h++) {
                for (int w = 0; w < page.GetLength(1);w++) {
                    page[h, w].Active = value;
                }
            }
        }
    }

    [IgnoreDataMember]
    [System.NonSerialized]
    BuildSignalManager buildSignalManager;
    public BuildSignalManager BuildSignalManager { get { return buildSignalManager; } set { buildSignalManager = value; } }

    //// Constructing from file (usually initializing puzzles)
    //public PageData(string filename) {
    //	BinaryFormatter bf = new BinaryFormatter ();
    //	TextAsset dataFile = Resources.Load<TextAsset> (filename);
    //	Stream s = new MemoryStream (dataFile.bytes);
    //	PageData pageData = (PageData)bf.Deserialize (s);
    //	page = pageData.Page;
    //	pageRotations = pageData.PageRotations;
    //}

    public PageData() {
        
    }

	// Initializing for test
	public PageData(int test) {

		sides = 4;

        active = false;

		System.Random random = new System.Random ();

		int height = 5;
		int width = 7;

		page = new RuneSlot[height, width];

		int[,] generation = new int[height, width];

		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				generation [i, j] = random.Next () % 3;
			}
		}

		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				if (generation [i, j] == 0) {
                    page[i, j] = new RuneSlot(new RuneData("Square Void"));
				} else {
                    page[i, j] = new RuneSlot(new RuneData("Square Empty"));
				}
			}
		}
	}

    public void FindNeighbors() {
        for (int h = 0; h < page.GetLength(0);h++) {
            for (int w = 0; w < page.GetLength(1);w++) {
                page[h, w].FindNeighbors(page);
            }
        }
    }


    public void SendEnergy()
    {
        for (int h = 0; h < page.GetLength(0); h++)
        {
            for (int w = 0; w < page.GetLength(1); w++)
            {
                page[h, w].SendEnergy(buildSignalManager);
            }
        }

    }

    public void ManipulateEnergy() {
        for (int h = 0; h < page.GetLength(0);h++)
        {
            for (int w = 0; w < page.GetLength(1);w++) 
            {
                page[h, w].RuneData.RuneTemplate.ManipulateEnergy(page[h, w], buildSignalManager);
            }
        }
    }

    public void Reset()
    {
        for (int h = 0; h < page.GetLength(0); h++)
        {
            for (int w = 0; w < page.GetLength(1); w++)
            {
                page[h, w].Reset();
            }
        }
    }

    public void SimulationStep()
    {
        SendEnergy();
        ManipulateEnergy();
    }

}