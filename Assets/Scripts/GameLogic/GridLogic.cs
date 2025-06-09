using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Backend of the grid game logic
public class GridLogic
{
    // Instantiate the cells
    CellLogic[,] cells;
    
    public GridLogic()
    {
        this.cells = new CellLogic[9, 9];
        //// Initialising the subgrid
        //int[] subgrid = {1, 1, 1, 2, 2, 2, 3, 3, 3
        //    , 1, 1, 1, 2, 2, 2, 3, 3, 3
        //    , 1, 1, 1, 2, 2, 2, 3, 3, 3
        //    , 4, 4, 4, 5, 5, 5, 6, 6, 6
        //    , 4, 4, 4, 5, 5, 5, 6, 6, 6
        //    , 4, 4, 4, 5, 5, 5, 6, 6, 6
        //    , 7, 7, 7, 8, 8, 8, 9, 9, 9
        //    , 7, 7, 7, 8, 8, 8, 9, 9, 9
        //    , 7, 7, 7, 8, 8, 8, 9, 9, 9};

        //// Fill the cells with given puzzle and solution
        //for (int i = 0; i < 9; i++)
        //{
        //    for (int j = 0; j < 9; j++)
        //    {
        //        //Console.WriteLine(i*9 + j);
        //        this.cells[i, j] = new CellLogic(puzzle[i * 9 + j], solution[i * 9 + j], subgrid[i * 9 + j]);
        //    }
        //}
    }

    public CellLogic[,] Cells
    {
        get { return this.cells; }
    }

    // Generate the grid with a random puzzle from the list
    public CellLogic[,] GenerateGrid(string filePath)
    {
        PuzzleReader reader = new PuzzleReader();
        Random rand = new Random();

        // Load puzzle from file path and store them
        reader.ReadCSV(filePath);
        List<int[]> puz = reader.Puzzle;
        List<int[]> sol = reader.Solution;

        // Choose a random puzzle and load it
        int r = rand.Next(puz.Count);

        int[] subgrid = {1, 1, 1, 2, 2, 2, 3, 3, 3
            , 1, 1, 1, 2, 2, 2, 3, 3, 3
            , 1, 1, 1, 2, 2, 2, 3, 3, 3
            , 4, 4, 4, 5, 5, 5, 6, 6, 6
            , 4, 4, 4, 5, 5, 5, 6, 6, 6
            , 4, 4, 4, 5, 5, 5, 6, 6, 6
            , 7, 7, 7, 8, 8, 8, 9, 9, 9
            , 7, 7, 7, 8, 8, 8, 9, 9, 9
            , 7, 7, 7, 8, 8, 8, 9, 9, 9};

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                this.cells[i, j] = new CellLogic(puz[r][i * 9 + j], sol[r][i * 9 + j], subgrid[i * 9 + j]);
            }
        }

        return this.cells;
    }
}
