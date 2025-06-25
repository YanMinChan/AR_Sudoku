using UnityEngine;

public class NumberController : MonoBehaviour
{
    [Range(1, 9)]
    public int number;
    private GridController _gridController;

    public void Initialize(GridController grid)
    {
        this._gridController = grid;
    }

    // Handles event on number pressed and print debug log
    public void OnNumberPressed()
    {
        this._gridController.FillNumber(CellController.currentlySelected, this.number);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
