using UnityEngine;

/// <summary>
/// Fill the grid with the puzzle
/// </summary>
public class GridController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string filePath = "./Assets/Resources/sudoku.csv"; // path of the dataset
        GridLogic grid = new GridLogic();
        CellLogic[,] cells = grid.GenerateGrid(filePath);
        if (cells != null)
        {
            Debug.Log("Cell loaded: " + cells.Length);
        } else
        {
            Debug.Log("AAAAAAAA");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
