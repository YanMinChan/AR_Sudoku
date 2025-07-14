using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class CellController : MonoBehaviour
{
    // Unity changable variables
    public static CellController currentlySelected; // The cell is selected by player
    [Range(1, 9)]
    public int editorRow;
    [Range(1, 9)]
    public int editorCol;

    // Dependency Injection
    private ISoundEffectDatabase _sfxDatabase;
    private INumberDatabase _numberDatabase;

    // Instance variables
    private CellModel _cellModel;
    private bool _isUnchangable = false;
    private GameObject _numberPrefab; // number in the cell
    private CellNumberController _numberController;

    // Constructor
    public void Init(ISoundEffectDatabase sfxDatabase, INumberDatabase numberDatabase)
    {
        _sfxDatabase = sfxDatabase;
        _numberDatabase = numberDatabase;
    }

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

    private void Awake(){}

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

        // Visual feedback
        _sfxDatabase.PlayAudio(3);
        HighlightCell("dark");
    }

    /// <summary>
    /// Instantiate a number GameObject to fill cell
    /// </summary>
    /// <param name="color"></param>
    /// <param name="init">If the number is part of puzzle</param>

    public void FillCell(string numColor, bool init=false, bool mute=false) {
        // If there is a number in the cell, destroy it
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        int number = this._cellModel.Num;
        GameObject prefab = _numberDatabase.GetNumber(number);
        if (prefab != null)
        {
            if (init) _isUnchangable = true;
            if (!mute) { _sfxDatabase.PlayAudio(2, 0.2f);}


            this._numberPrefab = Instantiate(prefab, transform);

            // Let CellNumberController handle filling in the number
            this._numberController = this._numberPrefab.AddComponent<CellNumberController>();
            this._numberController.SetNumber(number).SetColor(numColor);
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

    public CellController SetNumber(int num)
    {
        this._cellModel.Num = num;
        // this._numberController.IsDuplicate = isDuplicate;
        return this; // allow chaining
    }
}
