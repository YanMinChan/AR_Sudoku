using UnityEngine;

/// <summary>
/// Fill the grid with the puzzle
/// </summary>
public class GridController : MonoBehaviour
{
    CellController[,] cellControllers = new CellController[9, 9];
    CellModel[,] cells;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Load grid data from model
        GridModel grid = new GridModel();
        string filePath = "./Assets/Resources/sudoku.csv"; // path of the dataset
        this.cells = grid.GenerateGrid(filePath, 0);

        if (cells != null)
        {
            Debug.Log("Cell loaded: " + cells.Length);
            //Debug.Log(cells[0, 0].Solution);
            //Debug.Log(cells[0, 1].Solution);
            //Debug.Log(cells[0, 2].Solution);
            //Debug.Log(cells[0, 3].Solution);
        } 
        else
        {
            Debug.Log("AAAAAAAA");
        }
        BuildCellList();
    }

    // Update is called once per frame
    void Update()
    {
        // Update cell controller for change in grid
        for (int i=0; i<9; i++)
        {
            for (int j=0; j<9; j++)
            {
                int numbers = cells[i,j].Number;
                if (numbers != 0)
                {
                    cellControllers[i,j].FillNumber(numbers);
                }
            }
        }
    }

    // Build the list of cell controller by their row and col
    private void BuildCellList()
    {
        foreach (var controller in FindObjectsOfType<CellController>())
        {
            int r = controller.Row;
            int c = controller.Col;

            Debug.Log(r + " " + c);
            try
            {
                this.cellControllers[r - 1, c - 1] = controller;
            }
            catch { Debug.LogWarning(r + " " + c); }
        }
    }
}
