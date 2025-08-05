using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine.Assertions.Must;

// Backend of the grid game logic
public class GridModel
{
    // Instantiate the cells
    private CellModel[,] _cells;
    private int[] _puz; private int[] _sol;
    private int[] _numCount;

    private IGameLogger _logger;

    public GridModel(IGameLogger logger)
    {
        _cells = new CellModel[9, 9];
        _puz = new int[81];
        _sol = new int[81];
        _logger = logger;
    }

    public CellModel[,] Cells
    {
        get { return _cells; }
        set { _cells = value; }
    }

    public int[] Puz { get { return _puz; } }
    public int[] Sol { get { return _sol; } }

    public void Init(bool isNewPuzzle = false)
    {

        _logger.WriteToLog($"Begin Grid Model initialisation...");

        SelectPuzzle()
            .GenerateGrid(isNewPuzzle:isNewPuzzle);

        if (_cells != null)
            _logger.WriteToLog($"Cells loaded: {_cells.Length}");
        else
            _logger.WriteToLog("Cells not loaded!");

        _logger.WriteToLog($"Completed Grid Model initialisation \n\n");
    }

    /// <summary>
    /// Choose a random puzzle from the given file
    /// </summary>
    /// <param name="filePath"> file path </param>
    /// <param name="puzId"></param>
    /// <returns> the GridModel class </returns>
    public GridModel SelectPuzzle(int puzId = -1)
    {
        // Load puzzle from file path and store them
        PuzzleReader reader = new PuzzleReader(_logger);
        reader.Load();
        List<int[]> puzList = reader.Puzzle;
        List<int[]> solList = reader.Solution;

        // Choose a puzzle (random or a given ID)
        if (puzId != -1)
        {
            _puz = puzList[puzId];
            _sol = solList[puzId];
        } 
        else
        {
            Random rand = new Random();
            puzId = rand.Next(puzList.Count);
            _puz = puzList[puzId];
            _sol = solList[puzId];
        }

        // Log the puzzle id
        _logger.WriteToLog($"Puzzle ID: {puzId}");
        return this; // Allow chaining
    }

    /// <summary>
    /// Contruct the cell model
    /// </summary>
    /// <param name="puz"></param>
    /// <param name="sol"></param>
    /// <returns>The GridModel class</returns>
    public GridModel GenerateGrid(int[] puz = null, int[] sol = null, bool isReset = false, bool isNewPuzzle = false)
    {
        // For convenience of chaining
        if (puz == null) puz = _puz;
        if (sol == null) sol = _sol;

        int size = _cells.GetLength(0); // assume square grid

        // Construct the cell models by row and column
        for (int r = 0; r < size; r++)
        {
            for (int c = 0; c < size; c++)
            {
                if (isReset)
                {
                    _cells[r, c].Num = puz[r * 9 + c];   
                }
                else if (isNewPuzzle)
                {
                    _cells[r, c].Num = puz[r * 9 + c];
                    _cells[r, c].Sol = sol[r * 9 + c];
                }
                else
                {
                    _cells[r, c] = new CellModel(puz[r * 9 + c], sol[r * 9 + c], r, c);
                }
            }
        }
        return this; // Allow chaining
    }

    /// <summary>
    /// Duplicate check for number placed in the cell
    /// </summary>
    /// <param name="num"> number placed </param>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    public bool DuplicateExists(int num, int row, int col)
    {
        if (num == 0) return false; // Doesn't count duplicate for empty cell

        int size = _cells.GetLength(0); // assume square grid
        int sgridSize = 3;

        // Check for dup in col
        for (int c = 0; c < size; c++)
        {
            if (c == col) continue;
            if (_cells[row, c].Num == num) return true;
        }

        // Check for dup in row
        for (int r = 0; r < size; r++)
        {
            if (r == row) continue;
            if (_cells[r, col].Num == num) return true;
        }

        // Check for dup in subgrid
        int startRow = row - row % sgridSize;
        int startCol = col - col % sgridSize;

        for (int r = 0; r < sgridSize; r++)
        {
            for (int c = 0; c < sgridSize; c++)
            {
                int checkRow = r + startRow;
                int checkCol = c + startCol;
                if (checkRow == row && checkCol == col) continue;
                if (_cells[checkRow, checkCol].Num == num) return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Verify if the game is finished
    /// </summary>
    /// <returns></returns>
    public bool IsPuzzleFinished()
    {
        int size = _cells.GetLength(0); // assume square grid
        for (int r = 0; r < size; r++)
        {
            for (int c = 0; c < size; c++)
            {
                if (_cells[r, c].Num != _cells[r, c].Sol) return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Calculate the frequency of each digit and store them in _numCount
    /// </summary>
    public void CalculateDigitUsage()
    {
        // Refresh everytime the function is called
        _numCount = new int[9];
        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                int num = _cells[r, c].Num;
                if (_cells[r, c].Sol != num) continue;
                if (num >= 1 && num <= 9)
                {
                    _numCount[num - 1]++;
                }
            }
        }
    }

    // return true if all 9 digits of a number is filled in the right place
    public bool IsNumberFullyUsed(int num)
    {
        if (num == 0) return false;
        return _numCount[num - 1] >= 9;
    }

    public GridModel ResetGrid(bool newPuzzle = false)
    {
        if (newPuzzle) Init(isNewPuzzle:newPuzzle);
        else GenerateGrid(isReset:true); // Reset the cell model information

        return this; // allow chaining
    }

    // return true if there is duplicate
    public bool AnyDuplicateExists(int num)
    {
        for (int r = 0; r < _cells.GetLength(0); r++)
        {
            for (int c = 0; c < _cells.GetLength(1); c++)
            {
                int n = _cells[r, c].Num;
                if (num == n && DuplicateExists(num, r, c))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
