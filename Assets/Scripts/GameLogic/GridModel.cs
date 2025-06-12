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
    CellModel[,] cells;
    
    public GridModel()
    {
        this.cells = new CellModel[9, 9];
    }

    public CellModel[,] Cells
    {
        get { return this.cells; }
    }

    /// <summary>
    /// Choose a random puzzle from the given file
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="puzId"></param>
    /// <returns> the puzzle and solution in a tuple </returns>
    private (int[], int[]) puzzleSelector(string filePath, int puzId)
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
    public CellModel[,] GenerateGrid(string filePath, int puzId=-1)
    {
        // Choose a puzzle
        (int[] puz, int[] sol) = puzzleSelector(filePath, puzId);

        // Construct the cell models by row and column
        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                int subgrid = (r / 3) + (c / 3);
                this.cells[r, c] = new CellModel(puz[r * 9 + c], sol[r * 9 + c], subgrid, r, c);
            }
        }

        return this.cells;
    }

    public bool numberIsValid(int num, int row, int col)
    {
        // Check for dup in col
        for (int i = 0; i < 9; i++)
        {
            if (this.cells[row, i].num == num)
            {
                return false;
            }
        }
        // Check for dup in row
        for (int i = 0; i < 9; i++)
        {
            if (this.cells[i, col].num == num)
            {
                return false;
            }
        }
        // Check for dup in subgrid
        int subgrid = this.cells[row, col].sgrid; // extract the subgrid of user chosen cell
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (this.cells[i, j].sgrid == subgrid)
                {
                    if (this.cells[i, j].sgrid == num)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }
}
