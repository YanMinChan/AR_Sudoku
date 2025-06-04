using UnityEngine;

public class NumberBar : MonoBehaviour
{
    public GameObject numberPrefab;

    public void OnNumberPressed()
    {
        if (CellController.currentlySelected != null)
        {
            CellController.currentlySelected.FillNumber(numberPrefab);
            Debug.Log("Placed number in selected cell.");
        } else
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
