using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine.Assertions.Must;
using static UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics.HapticsUtility;

// Backend of the grid game logic
public class GridModel
{
    // Instantiate the cells
    private CellModel[,] _cells;
    private int[] _puz; private int[] _sol;
    private Stack<UndoAction> _actionStack;
    private int[] _numCount;

    public GridModel()
    {
        this._cells = new CellModel[9, 9];
        this._actionStack = new Stack<UndoAction>();
        this._puz = new int[81];
        this._sol = new int[81];
    }

    public CellModel[,] Cells
    {
        get { return this._cells; }
        set { this._cells = value; }
    }

    public int[] Puz { get { return this._puz; } }
    public int[] Sol { get { return this._sol; } }

    public void Init()
    {
        string filePath = "./Assets/Resources/sudoku.csv";
        this.SelectPuzzle(filePath)
            .GenerateGrid();

        if (this._cells != null)
            GameLog.Instance.WriteToLog($"(GridModel.cs) Cells loaded: {this._cells.Length}");
        else
            GameLog.Instance.WriteToLog("(GridModel.cs) Cells not loaded!");
    }

    /// <summary>
    /// Choose a random puzzle from the given file
    /// </summary>
    /// <param name="filePath"> file path </param>
    /// <param name="puzId"></param>
    /// <returns> the GridModel class </returns>
    public GridModel SelectPuzzle(string filePath, int puzId = -1)
    {
        // Load puzzle from file path and store them
        PuzzleReader reader = new PuzzleReader();
        reader.ReadCSV(filePath);
        List<int[]> puzList = reader.Puzzle;
        List<int[]> solList = reader.Solution;

        // Choose a puzzle (random or a given ID)
        if (puzId != -1)
        {
            this._puz = puzList[puzId];
            this._sol = solList[puzId];
        } 
        else
        {
            Random rand = new Random();
            puzId = rand.Next(puzList.Count);
            this._puz = puzList[puzId];
            this._sol = solList[puzId];
        }

        // Log the puzzle id
        GameLog.Instance.WriteToLog($"(GridModel.cs) Puzzle ID: {puzId}");
        return this; // Allow chaining
    }

    /// <summary>
    /// Contruct the cell model
    /// </summary>
    /// <param name="puz"></param>
    /// <param name="sol"></param>
    /// <returns>The GridModel class</returns>
    public GridModel GenerateGrid(int[] puz = null, int[] sol = null, bool isReset = false)
    {
        // For convenience of chaining
        if (puz == null) puz = this._puz;
        if (sol == null) sol = this._sol;

        int size = this._cells.GetLength(0); // assume square grid

        // Construct the cell models by row and column
        for (int r = 0; r < size; r++)
        {
            for (int c = 0; c < size; c++)
            {
                if (!isReset)
                {
                    this._cells[r, c] = new CellModel(puz[r * 9 + c], sol[r * 9 + c], r, c);
                }
                else
                {
                    this._cells[r, c].Num = puz[r * 9 + c];
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

        int size = this._cells.GetLength(0); // assume square grid
        int sgridSize = 3;

        // Check for dup in col
        for (int c = 0; c < size; c++)
        {
            if (c == col) continue;
            if (this._cells[row, c].Num == num) return true;
        }

        // Check for dup in row
        for (int r = 0; r < size; r++)
        {
            if (r == row) continue;
            if (this._cells[r, col].Num == num) return true;
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
                if (this._cells[checkRow, checkCol].Num == num) return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Verify if the game is finished
    /// </summary>
    /// <returns></returns>
    public bool IsGameFinished()
    {
        int size = this._cells.GetLength(0); // assume square grid
        for (int r = 0; r < size; r++)
        {
            for (int c = 0; c < size; c++)
            {
                if (this._cells[r, c].Num != this._cells[r, c].Sol) return false;
            }
        }
        return true;
    }

    public bool TryPopLastAction(out UndoAction action)
    {
        return this._actionStack.TryPop(out action);
    }

    public void PushLastAction(int num, int row, int col, string color)
    {
        this._actionStack.Push(new UndoAction
        {
            num = num,
            row = row,
            col = col,
            numColor = color
        });
    }

    // Calculate the number of time the digit is used
    public void CalculateDigitUsage()
    {
        // Refresh everytime the function is called
        this._numCount = new int[9];
        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                int num = this._cells[r, c].Num;
                if (this._cells[r, c].Sol != num) continue;
                if (num >= 1 && num <= 9)
                {
                    this._numCount[num - 1]++;
                }
            }
        }
    }

    public bool IsNumberFullyUsed(int num)
    {
        if (num == 0) return false;
        return this._numCount[num - 1] >= 9;
    }

    public GridModel ResetGrid()
    {
        GenerateGrid(isReset:true); // Reset the cell model information
        this._actionStack = new Stack<UndoAction>(); // Reset the undo stack
        return this; // allow chaining
    }
}
