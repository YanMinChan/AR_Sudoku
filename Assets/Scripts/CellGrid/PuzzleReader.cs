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
    private List<int[]> _puzzle;
    private List<int[]> _solution;

    private static string _filePath = AppPaths.PuzzleFile;

    // Constructor
    public PuzzleReader()
    {
        this._puzzle = new List<int[]>();
        this._solution = new List<int[]>();
    }
    public List<int[]> Puzzle
    {
        get { return this._puzzle; }
        set { this._puzzle = value; }
    }

    public List<int[]> Solution
    {
        get { return this._solution; }
        set { this._solution = value; }
    }

    // Public methods
    public void Load(int numPuz = 100)
    {
        // Read from csv
        using (StreamReader sr = new StreamReader(_filePath))
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
                    this._puzzle.Add(puzSet);
                    this._solution.Add(solSet);

                    data = sr.ReadLine();
                    numPuz--;
                }
                GameLogger.Instance.WriteToLog($"(PuzzleReader.cs) Puzzle loaded from {_filePath}");
            }
            catch (FileNotFoundException fnf)
            {
                GameLogger.Instance.WriteToLog($"(PuzzleReader.cs) Error on loading file: {fnf.Message}");
            }
            catch (Exception e)
            {
                GameLogger.Instance.WriteToLog($"(PuzzleReader.cs) Error on loading file: {e.Message}");
            }
        }
    }
}
