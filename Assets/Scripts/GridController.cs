using UnityEngine;

/// <summary>
/// Fill the grid with the puzzle
/// </summary>
public class GridController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PuzzleReader reader = new PuzzleReader();
        reader.ReadCSV("./Assets/Resoruces/sudoku.csv");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
