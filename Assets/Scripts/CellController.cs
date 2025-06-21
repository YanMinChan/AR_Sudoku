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

    // Get set method
    public CellModel Model
    {
        get { return cellModel; }
        set { cellModel = value; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){}

    // Update is called once per frame
    void Update(){}

    /// <summary>
    /// Select the current cell
    /// </summary>
    public void SelectThisCell()
    {
        // Remove highlight on previous cell
        if (currentlySelected != null)
        {
            RemoveHighlightCell();
        }
        
        currentlySelected = this;
        Debug.Log("Selected cell:" + gameObject.name);

        HighlightCell("dark");
    }

    /// <summary>
    /// Generate a number object to fill cell
    /// </summary>
    public void FillNumber(int number, string color) {
        
        // If there is a number in the cell, destroy it
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        // Instantiate the number
        GameObject prefab = NumberDatabase.Instance.GetNumber(number);
        if (prefab != null)
        { 
            // Play sound effect
            SoundEffectDatabase.Instance.PlayAudio(1);

            // Number prefab transform setup
            this.numberPrefab = Instantiate(prefab, transform);
            this.numberPrefab.transform.localPosition = Vector3.zero;
            this.numberPrefab.transform.localRotation = Quaternion.Euler(0, 180, 0);
            this.numberPrefab.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            var rend = this.numberPrefab.GetComponent<Renderer>();
            if (color == "black")
                rend.material = Resources.Load("Materials/Number_Black_Mat", typeof(Material)) as Material;
            else if (color == "red")
            {
                Debug.Log("CellController.cs: I am red!");
                rend.material = Resources.Load("Materials/Number_Red_Mat", typeof(Material)) as Material;
            }
            else if (color == "blue")
                rend.material = Resources.Load("Materials/Number_Blue_Mat", typeof(Material)) as Material;
        }
    }

    /// <summary>
    /// Highlight the cell with another material
    /// </summary>
    public void HighlightCell(string color)
    {
        Renderer rend = currentlySelected.GetComponent<Renderer>();
        if (rend != null)
        {
            if (color == "dark")
                rend.material = Resources.Load("Materials/Cell_Transparent_DarkerHighlight_Mat", typeof(Material)) as Material;
            else if (color == "red")
                rend.material = Resources.Load("Materials/Cell_Transparent_RedHighlight_Mat", typeof(Material)) as Material;
        }
    }

    public void RemoveHighlightCell()
    {
        Renderer rend = currentlySelected.GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material = Resources.Load("Materials/Cell_Transparent_Mat", typeof(Material)) as Material;
        }
    }

    /// <summary>
    /// Highlight the number with another material
    /// red: invalid, blue: user filled number, black: puzzle
    /// </summary>
    public void HighlightNumber(string color)
    {
        Renderer rend = this.numberPrefab.GetComponent<Renderer>();
        if (rend != null)
        {
            if (color == "red")
            {
                Debug.Log("CellController.cs: I am red!");
                rend.material = Resources.Load("Materials/Number_Red_Mat", typeof(Material)) as Material;
            }
        }

    }
}
