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

    private IGameLogger _logger;

    private static string _filePath = AppPaths.PuzzleFile;

    // Constructor
    public PuzzleReader(IGameLogger logger)
    {
        _puzzle = new List<int[]>();
        _solution = new List<int[]>();

        _logger = logger;
    }
    public List<int[]> Puzzle
    {
        get { return _puzzle; }
        set { _puzzle = value; }
    }

    public List<int[]> Solution
    {
        get { return _solution; }
        set { _solution = value; }
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
                string data = "";
                while ((data = sr.ReadLine()) != null && numPuz != 0)
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
                    _puzzle.Add(puzSet);
                    _solution.Add(solSet);
                    numPuz--;
                }
                GameLogger.Instance.WriteToLog($"Puzzle loaded from {_filePath}");
                GameLogger.Instance.WriteToLog($"Puzzle count: {_puzzle.Count}");
            }
            catch (FileNotFoundException fnf)
            {
                GameLogger.Instance.WriteToLog($"Error on loading file: {fnf.Message}");
            }
            catch (Exception e)
            {
                GameLogger.Instance.WriteToLog($"Error on loading file: {e.Message}");
            }
        }
    }
}
