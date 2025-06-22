using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Read puzzle and store them into lists
/// </summary>
public class PuzzleReader
{
    // Instance variable
    List<int[]> puzzle;
    List<int[]> solution;

    // Constructor
    public PuzzleReader()
    {
        this.puzzle = new List<int[]>();
        this.solution = new List<int[]>();
    }

    // Public methods
    public void ReadCSV(string filePath, int numPuz = 100)
    {
        // Read from csv
        using (StreamReader sr = new StreamReader(filePath))
        {
            try
            {
                string[] gameSet; //puzzle and solution

                string header = sr.ReadLine(); // skip the header
                string data = sr.ReadLine();
                while ((data != null) && numPuz != 0)
                {
                    // obtain the puzzle and solution
                    gameSet = data.Split(',');
                    int[] puzSet = new int[81];
                    int[] solSet = new int[81];
                    // parse puzzle and solution to array
                    for (int j = 0; j < 81; j++)
                    {
                        // convert char to int by ascii
                        puzSet[j] = gameSet[0][j] - '0';
                        solSet[j] = gameSet[1][j] - '0';
                    }
                    this.puzzle.Add(puzSet);
                    this.solution.Add(solSet);

                    data = sr.ReadLine();
                    numPuz--;
                }
                GameLog.Instance.WriteToLog("Read puzzle");
            }
            catch (FileNotFoundException fnf)
            {
                Console.WriteLine("Error on file path, file not found!");
            }
        }
    }

    public List<int[]> Puzzle
    {
        get { return puzzle; }
        set { puzzle = value; }
    }

    public List<int[]> Solution
    {
        get { return solution; }
        set { solution = value; }
    }
}
