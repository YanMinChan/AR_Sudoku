using JetBrains.Annotations;
using UnityEngine;

public class CellController : MonoBehaviour
{
    // Unity changable variables
    public static CellController currentlySelected; // The cell is selected by player
    [Range(1, 9)]
    public int editorRow;
    [Range(1, 9)]
    public int editorCol;

    // Instance variables
    private CellModel cellModel;
    [HideInInspector]
    public bool isDefaultCell = false;
    [HideInInspector]
    public GameObject numberPrefab;

    // Constructor
    public CellController(){}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){}

    // Update is called once per frame
    void Update()
    {

    }

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
            this.numberPrefab = Instantiate(prefab, transform);
            this.numberPrefab.transform.localPosition = Vector3.zero;
            this.numberPrefab.transform.localRotation = Quaternion.Euler(0, 180, 0);
            this.numberPrefab.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
    }

    public CellModel Model
    {
        get { return cellModel; }
        set { cellModel = value; }
    }
}
