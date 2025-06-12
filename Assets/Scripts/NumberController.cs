using UnityEngine;

public class NumberController : MonoBehaviour
{
    [Range(1, 9)]
    public int number;
    private GridController gridController;

    public void Initialize(GridController grid)
    {
        this.gridController = grid;
    }

    public void OnNumberPressed()
    {
        Debug.Log("AAA");
        if (CellController.currentlySelected != null && !CellController.currentlySelected.isDefaultCell)
        {
            if (this.gridController.FillNumber(CellController.currentlySelected, this.number))
            {
                Debug.Log("Placed number in selected cell.");
            } 
            
        } 
        else if (CellController.currentlySelected.isDefaultCell)
        {
            Debug.Log("The cell cannot be changed");
            
        }
        else 
        {
            Debug.Log("No cell is selected");
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
