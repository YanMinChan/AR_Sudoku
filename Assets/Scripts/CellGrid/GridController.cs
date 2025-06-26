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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Init()
    {
        // Instantiate the model
        this._gridModel = new GridModel();
        this._cellControllers = new CellController[9, 9];

        // Instantiate everything
        //InstantiateCellModels();
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
                controller.Model = this._gridModel.Cells[r-1, c-1];
                this._cellControllers[r-1, c-1] = controller;
            }
            catch { Debug.LogWarning((r-1) + " " + (c-1)); }
        }
    }

    // Assign grid to number controller
    private void NumberControllersInit()
    {
        this._numberControllers = FindObjectsByType<NumberController>(FindObjectsSortMode.None).ToList();
        foreach (var cont in this._numberControllers)
        {
            cont.Initialize(this);
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
        GameLog.Instance.WriteToLog($"(GridController.cs) Fill number {number} in [{model.Row}, {model.Col}]");
    }

    // Handle undo button event
    public void UndoLastAction()
    {
        if (this._gridModel.TryPopLastAction(out UndoAction action))
        {
            this._cellControllers[action.row, action.col]
                .UpdateModel(action.num)
                .FillNumber(action.numColor);
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

    public void DisableCompletedNumber(int number)
    {
    }

    // Function from GridModel
    public bool IsGameFinished() { return this._gridModel.IsGameFinished(); }
}
