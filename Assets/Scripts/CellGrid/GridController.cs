using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics.HapticsUtility;

/// <summary>
/// GridController class
/// Manages GridModel, and is a collection of CellControllers and NumberControllers
/// </summary>
public class GridController : MonoBehaviour
{
    // Instance variables
    private GridModel _gridModel; 
    private CellController[,] _cellControllers;
    private List<NumberController> _numberControllers;

    public GridModel GetGridModel() {  return _gridModel; }
    public CellController[,] GetCellControllers() {  return _cellControllers; }
    public List<NumberController> GetNumberControllers() { return _numberControllers; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){}

    // Update is called once per frame
    void Update(){}

    public void Init()
    {
        // Instantiate the model
        this._gridModel = new GridModel();
        this._cellControllers = new CellController[9, 9];
        this._numberControllers = new List<NumberController>();

        // Instantiate everything
        this._gridModel.Init();
        CellControllersInit();
        NumberControllersInit();

        // Generate the grid view
        BuildGrid();
    }

    // Find all cell controllers object and assign them to their respective model
    private void CellControllersInit()
    {
        foreach (var controller in FindObjectsByType<CellController>(FindObjectsSortMode.None))
        {
            int r = controller.editorRow;
            int c = controller.editorCol;
            
            // Connect the cell model and controller
            try
            {
                controller.Init(SoundEffectDatabase.Instance, NumberDatabase.Instance);
                controller.Model = this._gridModel.Cells[r-1, c-1];
                this._cellControllers[r-1, c-1] = controller;
            }
            catch { Debug.LogWarning((r-1) + " " + (c-1)); }
        }
    }

    // Assign grid to number controller
    private void NumberControllersInit()
    {
        var numberBar = GameObject.FindGameObjectsWithTag("NumberBar");

        foreach (var number in numberBar) 
        {
            var controller = number.GetComponentInChildren<NumberController>();
            this._numberControllers.Add(controller);
            controller.Initialize(this);
        }
    }

    // Build a new puzzle
    private void BuildGrid()
    {
        // Update cell controller for change in grid
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                this._cellControllers[i, j].FillNumber("black", init:true);
            }
        }
        GameLog.Instance.WriteToLog("(GridController.cs) Grid gameObject built.");
    }

    // Fill number in the selected cell
    public void FillNumber(int number)
    {
        CellController cellCtr = CellController.currentlySelected;

        // Handled failure event
        if (GameManager.Instance.IsTimerPaused())
        {
            Debug.Log("GridController.cs: Game is paused!");
            return;
        }
        else if (cellCtr == null)
        {
            Debug.Log("GridController.cs: No cell is selected");
            return;
        }
        else if (cellCtr.IsUnchangable)
        {
            Debug.Log("GridController.cs: The cell cannot be changed");
            return;
        }

        // Save previous state
        PushUndoState(cellCtr);

        // Check for duplicate
        CellModel model = cellCtr.Model;
        bool dup = this._gridModel.DuplicateExists(number, model.Row, model.Col);
        string newColor = NumberColor(dup);

        // Update cell model
        cellCtr.UpdateModel(number).FillNumber(newColor);

        // Update GridModel
        this._gridModel.CalculateDigitUsage();

        UpdateNumberBarVisibility();
        UpdateNumberColor();

        GameLog.Instance.WriteToLog($"(GridController.cs) Fill number {number} in [{model.Row}, {model.Col}]");
    }

    // Handle undo button event
    public void UndoLastAction()
    {
        if (GameManager.Instance.IsTimerPaused())
        {
            Debug.Log("GridController.cs: Game is paused!");
            return;
        }

        if (this._gridModel.TryPopLastAction(out UndoAction action))
        {
            this._cellControllers[action.row, action.col]
                .UpdateModel(action.num)
                .FillNumber(action.numColor);

            // Update GridModel
            this._gridModel.CalculateDigitUsage();
            UpdateNumberBarVisibility();
            UpdateNumberColor();
        }
        else
        {
            Debug.Log("GridController.cs: Nothing to undo");
            return;
        }

        GameLog.Instance.WriteToLog($"(GridController.cs) Undo Number {action.num} in [{action.row}, {action.col}]");
    }

    public void ResetGrid()
    {
        // Clear game status
        this._gridModel.ResetGrid();

        // Rebuild the grid
        BuildGrid();

        GameLog.Instance.WriteToLog($"(GridController.cs) Game restarted.");
    }

    /// <summary>
    /// Save previous controller and number state in stack
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="model"></param>
    private void PushUndoState(CellController controller)
    {
        CellModel model = controller.Model;

        // Load & store previous information
        bool duplicateExist = this._gridModel.DuplicateExists(model.Num, model.Row, model.Col);
        string previousColor = NumberColor(duplicateExist);

        // Debug.Log($"[PUSH] At push time - num: {previousNum}, color: {previousColor}");
        this._gridModel.PushLastAction(model.Num, model.Row, model.Col, previousColor);
    }

    // Determine color of the number
    private string NumberColor(bool duplicateExist)
    {
        if (duplicateExist) return "red";
        else return "blue";
    }

    public void UpdateNumberBarVisibility()
    { 
        for (int i = 1; i <= 9; i++)
        {
            NumberController numCtr = this._numberControllers.FirstOrDefault(n => n.number == i);
            bool numUsed = this._gridModel.IsNumberFullyUsed(i);
            bool anyDuplicate = this._gridModel.AnyDuplicateExists(i);
            bool objectVisibility = numUsed && !anyDuplicate;
            numCtr.SetNumberGameObjectVisibility(objectVisibility);
            // Debug.Log("" + numCtr.number + numUsed);
        }
    }

    public void UpdateNumberColor()
    {
        for (int r = 0; r < this._cellControllers.GetLength(0); r++)
        {
            for (int c = 0; c < this._cellControllers.GetLength(1); c++)
            {
                if (this._cellControllers[r, c].IsUnchangable == true) continue;
                int n = this._cellControllers[r, c].Model.Num;
                bool duplicateExist = this._gridModel.DuplicateExists(n, r, c);
                string color = NumberColor(duplicateExist);
                this._cellControllers[r, c].FillNumber(color);
            }
        }
    }

    // Function from GridModel
    public bool IsGameFinished() { return this._gridModel.IsGameFinished(); }
}
