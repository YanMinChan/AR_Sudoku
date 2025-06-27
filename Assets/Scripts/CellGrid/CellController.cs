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
    private CellModel _cellModel;
    private bool _isUnchangable = false;
    private GameObject _numberPrefab; // number in the cell

    // Constructor
    public CellController(){}

    // Get set method
    public CellModel Model
    {
        get { return _cellModel; }
        set { _cellModel = value; }
    }

    public bool IsUnchangable
    {
        get { return _isUnchangable; }
        set { _isUnchangable = value; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){}

    // Update is called once per frame
    void Update(){}

    // Handle events

    /// <summary>
    /// Select the current cell event
    /// </summary>
    public void SelectThisCell()
    {
        // Remove highlight on previous cell
        if (currentlySelected != null)
        {
            RemoveHighlightCell();
        }
        
        currentlySelected = this;

        // Audio and visual feedback
        SoundEffectDatabase.Instance.PlayAudio(3);
        HighlightCell("dark");
    }

    /// <summary>
    /// Instantiate a number GameObject to fill cell
    /// </summary>
    /// <param name="number"></param>
    /// <param name="color"></param>
    /// <param name="init">If the number is part of puzzle</param>

    public void FillNumber(string color, bool init=false) {
        int number = this._cellModel.Num;
        // If there is a number in the cell, destroy it
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        // Instantiate the number
        this._cellModel.Num = number;
        GameObject prefab = NumberDatabase.Instance.GetNumber(number);
        if (prefab != null) // Handles empty cell for 0
        { 
            if (!init) SoundEffectDatabase.Instance.PlayAudio(2); // Only play sfx when it is user filling in the number
            else this._isUnchangable = true; // Set cell to unchangeable if it is part of puzzle
            
            // Number prefab transform and material setup
            this._numberPrefab = Instantiate(prefab, transform);
            this._numberPrefab.transform.localPosition = Vector3.zero;
            this._numberPrefab.transform.localRotation = Quaternion.Euler(0, 180, 0);
            this._numberPrefab.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            this._numberPrefab.GetComponent<NumberController>().enabled = false; // disable number controller script to avoid misclick and throwing error

            InstantiateNumberMaterial(color);
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
            else if (color == "red") // For now we don't use the red highlight yet
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

    // Helper functions

    /// <summary>
    /// Highlight the number with different color material
    /// red: invalid, blue: user filled number, black: puzzle
    /// </summary>
    private void InstantiateNumberMaterial(string color)
    {
        Renderer rend = this._numberPrefab.GetComponent<Renderer>();
        if (rend != null)
        {
            switch (color) { 
                case "red":
                    rend.material = Resources.Load("Materials/Number_Red_Mat", typeof(Material)) as Material;
                    break;
                case "black":
                    rend.material = Resources.Load("Materials/Number_Black_Mat", typeof(Material)) as Material;
                    break;
                case "blue":
                    rend.material = Resources.Load("Materials/Number_Blue_Mat", typeof(Material)) as Material;
                    break;
            }
        }
    }

    public CellController UpdateModel(int num)
    {
        this._cellModel.Num = num;
        return this; // allow chaining
    }
}
