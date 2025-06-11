using JetBrains.Annotations;
using UnityEngine;

public class CellController : MonoBehaviour
{
    // Unity changable variables
    public static CellController currentlySelected; // The cell is selected by player
    [Range(1, 9)]
    public int row;
    [Range(1, 9)]
    public int col;

    private CellModel cellModel;

    private bool isDefaultCell = false;

    // Constructor
    public CellController()
    {
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){}

    /// <summary>
    /// Select the current cell
    /// </summary>
    public void SelectThisCell()
    {
        currentlySelected = this;
        Debug.Log("Selected cell:" + gameObject.name);
    }

    /// <summary>
    /// Generate a number object to fill cell
    /// </summary>
    public void FillNumber(int number) {

        // If there is a number in the cell, destroy it
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        // Instantiate the number
        GameObject prefab = NumberDatabase.Instance.GetNumber(number);
        if (prefab != null)
        {
            GameObject numberPrefab = Instantiate(prefab, transform);
            numberPrefab.transform.localPosition = Vector3.zero;
            numberPrefab.transform.localRotation = Quaternion.Euler(0, 180, 0);
            numberPrefab.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }

        // Update the cell model
        cellModel.Number = number;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int Row
    {
        get { return this.row; }
    }

    public int Col
    {
        get { return this.col; }
    }

    public CellModel CellModel
    {
        set { cellModel = value; }
    }

    public bool IsDefaultCell
    {
        get { return this.isDefaultCell; }
        set { isDefaultCell = value; }
    }
}
