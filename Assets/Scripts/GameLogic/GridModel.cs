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
    
    public GridModel()
    {
        this._cells = new CellModel[9, 9];
    }

    public CellModel[,] Cells
    {
        get { return this._cells; }
        set { this._cells = value; }
    }

    /// <summary>
    /// Choose a random puzzle from the given file
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="puzId"></param>
    /// <returns> the puzzle and solution in a tuple </returns>
    public (int[], int[]) puzzleSelector(string filePath, int puzId = -1)
    {
        // Load puzzle from file path and store them
        PuzzleReader reader = new PuzzleReader();
        reader.ReadCSV(filePath);
        List<int[]> puzList = reader.Puzzle;
        List<int[]> solList = reader.Solution;

        // Choose a puzzle (random or a given ID)
        if (puzId != -1)
        {
            return (puzList[puzId], solList[puzId]);
        } 
        else
        {
            Random rand = new Random();
            int r = rand.Next(puzList.Count);
            return (puzList[r], solList[r]);
        }
    }

    // Construct the grid model
    public CellModel[,] GenerateGrid(int[] puz = null, int[] sol = null)
    {
        //// Choose a puzzle
        //(int[] puz, int[] sol) = puzzleSelector(filePath, puzId);

        // For convenience of generating empty grid
        if (puz == null)
        {
            puz = new int[81];
        }
        if (sol == null)
        {
            sol = new int[81];
        }

        // Construct the cell models by row and column
        for (int r = 0; r < this._cells.GetLength(0); r++)
        {
            for (int c = 0; c < this._cells.GetLength(1); c++)
            {
                this._cells[r, c] = new CellModel(puz[r * 9 + c], sol[r * 9 + c], r, c);
            }
        }

        return this._cells;
    }

    public int numberOfDuplicate(int num, int row, int col)
    {
        int numDup = 0;

        if (num == 0)
        {
            return numDup;
        }
        // Check for dup in col
        // Update cell model for each duplicate
        for (int c = 0; c < this._cells.GetLength(1); c++)
        {
            if (this._cells[row, c].num == num && c != col)
            {
                numDup++;
            }
        }
        // Check for dup in row
        for (int r = 0; r < this._cells.GetLength(0); r++)
        {
            if (this._cells[r, col].num == num && r != row)
            {
                numDup++;
            }
        }
        // Check for dup in subgrid
        int sgridSize = 3; //subgrid is 3x3
        int startRow = row - row % sgridSize, startCol = col - col % sgridSize; // extract the subgrid of user chosen cell
        for (int r = 0; r < sgridSize; r++)
        {
            for (int c = 0; c < sgridSize; c++)
            {
                if (this._cells[r + startRow, c + startCol].num == num && (r + startRow != row) && (c + startCol != col))
                {
                    numDup++;
                }
            }
        }
        return numDup;
    }

    public bool gameFinished()
    {
        for (int r = 0; r < this._cells.GetLength(0); r++)
        {
            for (int c = 0; c < this._cells.GetLength(1); c++)
            {
                if (this._cells[r, c].num != this._cells[r, c].sol)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
