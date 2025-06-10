using UnityEngine;

/// <summary>
/// Fill the grid with the puzzle
/// </summary>
public class GridController : MonoBehaviour
{
    CellController[,] cellControllers;
    CellModel[,] cells;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Load grid data from model
        GridModel grid = new GridModel();
        string filePath = "./Assets/Resources/sudoku.csv"; // path of the dataset
        this.cells = grid.GenerateGrid(filePath);

        if (cells != null)
        {
            Debug.Log("Cell loaded: " + cells.Length);
            //Debug.Log(cells[0, 0].Solution);
            //Debug.Log(cells[0, 1].Solution);
            //Debug.Log(cells[0, 2].Solution);
            //Debug.Log(cells[0, 3].Solution);
        } else
        {
            Debug.Log("AAAAAAAA");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update cell controller for change in grid
        for (int i=0; i<9; i++)
        {
            for (int j=0; j<9; j++)
            {
                int numbers = cells[i,j].Numbers;
                if (numbers != 0)
                {
                    cellControllers[i,j].FillNumbee(numbers);
                }
            }
        }
    }
}
