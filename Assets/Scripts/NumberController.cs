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
        if (CellController.currentlySelected != null && !CellController.currentlySelected.IsUnchangable)
        {
            this._gridController.FillNumber(CellController.currentlySelected, this.number);
            Debug.Log("NumberController.cs: Placed number in selected cell.");
        } 
        else if (CellController.currentlySelected.IsUnchangable)
        {
            Debug.Log("NumberController.cs: The cell cannot be changed");
            
        }
        else 
        {
            Debug.Log("NumberController.cs: No cell is selected");
        }
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
