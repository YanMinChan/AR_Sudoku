using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Backend of the grid game logic
public class GridModel
{
    // Instantiate the cells
    private CellModel[,] _cells;
    private int[] _puz; private int[] _sol;

    public GridModel()
    {
        this._cells = new CellModel[9, 9];
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
            int r = rand.Next(puzList.Count);
            this._puz = puzList[r];
            this._sol = solList[r];
        }
        return this; // Allow chaining
    }

    /// <summary>
    /// Contruct the cell model
    /// </summary>
    /// <param name="puz"></param>
    /// <param name="sol"></param>
    /// <returns>The GridModel class</returns>
    public GridModel GenerateGrid(int[] puz = null, int[] sol = null)
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
                this._cells[r, c] = new CellModel(puz[r * 9 + c], sol[r * 9 + c], r, c);
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
            if (this._cells[row, c].num == num) return true;
        }

        // Check for dup in row
        for (int r = 0; r < size; r++)
        {
            if (r == row) continue;
            if (this._cells[r, col].num == num) return true;
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
                if (this._cells[checkRow, checkCol].num == num) return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Verify if the game is finished
    /// </summary>
    /// <returns></returns>
    public bool GameFinished()
    {
        int size = this._cells.GetLength(0); // assume square grid
        for (int r = 0; r < size; r++)
        {
            for (int c = 0; c < size; c++)
            {
                if (this._cells[r, c].num != this._cells[r, c].sol) return false;
            }
        }
        return true;
    }
}
