using JetBrains.Annotations;
using System;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// MonoBehaviour script for Cell GameObject
/// </summary>
public class CellController : MonoBehaviour
{
    // Unity changable variables
    public static CellController currentlySelected; // The cell selected by player
    [Range(1, 9)]
    public int editorRow;
    [Range(1, 9)]
    public int editorCol;

    // Dependency Injection
    private ISoundEffectDatabase _sfxDatabase;
    private INumberDatabase _numberDatabase;
    private IGameLogger _logger;

    // Instance variables
    private CellModel _cellModel;
    private bool _isUnchangable = false;
    private GameObject _numberPrefab; // number in the cell
    private CellNumberController _numberController;

    // Constructor
    public void Init(ISoundEffectDatabase sfxDatabase, INumberDatabase numberDatabase, IGameLogger logger)
    {
        _sfxDatabase = sfxDatabase;
        _numberDatabase = numberDatabase;
        _logger = logger;
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

        try
        {
            currentlySelected = this;
            _logger.WriteToLog($"Selected cell [{editorRow}, {editorCol}]");

            // Visual feedback
            _sfxDatabase.PlayAudio(3);
            HighlightCell();
        } 
        catch (Exception ex)
        {
            _logger.WriteToLog($"Error on selecting cell [{editorRow}, {editorCol}]: {ex.Message}");
        }
    }

    /// <summary>
    /// Instantiate a number GameObject to fill cell
    /// </summary>
    /// <param name="color"></param>
    /// <param name="init">If the number is part of puzzle</param>

    public void FillCell(string numColor, bool init=false, bool muteSfx=false) {
        // If there is a number in the cell, destroy it
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        
        int number = this._cellModel.Num;
        GameObject prefab = _numberDatabase.GetNumber(number);
        if (prefab != null)
        {
            try
            {
                if (init) _isUnchangable = true; // default cell is unchangeable
                if (!muteSfx) { _sfxDatabase.PlayAudio(2, 0.5f); }

                _numberPrefab = Instantiate(prefab, transform);

                // Let CellNumberController handle filling in the number
                _numberController = _numberPrefab.AddComponent<CellNumberController>();
                _numberController.SetNumber(number).SetColor(numColor);

                _logger.WriteToLog($"Filled in number {number} in [{editorRow}, {editorCol}]");
            }
            catch (Exception ex)
            {
                _logger.WriteToLog($"Error on filling number {number} in [{editorRow}, {editorCol}]: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Highlight the cell with another material
    /// </summary>
    public void HighlightCell()
    {
        Renderer rend = currentlySelected.GetComponent<Renderer>();

        if (rend != null)
        {
            rend.material = Resources.Load("Materials/Cell_Transparent_DarkerHighlight_Mat", typeof(Material)) as Material;
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
        return this; // allow chaining
    }
}
