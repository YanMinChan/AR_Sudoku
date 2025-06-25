using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// The manager class (and controller for Grid)
/// </summary>
public class GridController : MonoBehaviour
{
    // Instance variables
    private GridModel _gridModel; 
    private CellModel[,] _cellModels;
    private CellController[,] _cellControllers;
    private List<NumberController> _numberControllers;
    private Stack<UndoAction> _actionStack;
    private TimerController _timerController;
    private bool _hasGameCompleted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Instantiate some variables
        this._gridModel = new GridModel();
        this._actionStack = new Stack<UndoAction>();

        // Load puzzle data from CSV to cell models
        InstantiateCellModels();

        // Manage other models and controllers (assigned in Unity)
        InstantiateCellControllers();
        InstantiateNumberControllers();
        InstantiateTimerController();

        // Generate the grid view
        BuildGrid();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the game is finished
        if (!this._hasGameCompleted && this._gridModel.IsGameFinished())
        {
            this._hasGameCompleted = true;
            // TODO: Change to on scene feedback
            Debug.Log("YOU WINNNNN");
            GameLog.Instance.WriteToLog("(GridController.cs) The game is finished.");
        }
    }

    // Load data from csv to cell model
    private void InstantiateCellModels()
    {
        // Instantiate cell model
        string filePath = "./Assets/Resources/sudoku.csv"; // path of the dataset
        this._cellModels = this._gridModel.SelectPuzzle(filePath)
            .GenerateGrid()
            .Cells;

        if (this._cellModels != null)
            GameLog.Instance.WriteToLog($"(GridController.cs) Cells loaded: {this._cellModels.Length}");
        else
            GameLog.Instance.WriteToLog("(GridController.cs) Cells not loaded!");
    }

    // Find all cell controllers object and assign them to their respective model
    private void InstantiateCellControllers()
    {
        this._cellControllers = new CellController[9, 9];
        foreach (var controller in FindObjectsByType<CellController>(FindObjectsSortMode.None))
        {
            int r = controller.editorRow;
            int c = controller.editorCol;
            
            // Connect the cell model and controller
            try
            {
                controller.Model = this._cellModels[r-1, c-1];
                this._cellControllers[r-1, c-1] = controller;
            }
            catch { Debug.LogWarning((r-1) + " " + (c-1)); }
        }
    }

    // Assign grid to number controller
    private void InstantiateNumberControllers()
    {
        this._numberControllers = FindObjectsByType<NumberController>(FindObjectsSortMode.None).ToList();
        foreach (var cont in this._numberControllers)
        {
            cont.Initialize(this);
        }
    }

    private void InstantiateTimerController()
    {
        this._timerController = FindObjectsByType<TimerController>(FindObjectsSortMode.None)[0]; // There should be only one timer controller at the menu
    }

    // Build a new puzzle
    private void BuildGrid()
    {
        // Update cell controller for change in grid
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                int numbers = this._cellModels[i, j].Num;
                this._cellControllers[i, j].FillNumber(numbers, "black", init:true);
            }
        }
        GameLog.Instance.WriteToLog("(GridController.cs) Grid gameObject built.");
    }

    // Fill number in the selected cell
    public void FillNumber(CellController controller, int newNumber)
    {
        // Handled failure event
        if (this._timerController.IsPaused())
        {
            Debug.Log("GridController.cs: Game is paused!");
            return;
        }
        else if (CellController.currentlySelected == null)
        {
            Debug.Log("GridController.cs: No cell is selected");
            return;
        }
        else if (CellController.currentlySelected.IsUnchangable)
        {
            Debug.Log("GridController.cs: The cell cannot be changed");
            return;
        }

        // Load the cell model assigned to the controller
        CellModel model = controller.Model;

        // Save previous state
        PushUndoState(controller, model);

        // Apply new number
        model.Num = newNumber;

        // Determine number and update view
        bool dup = this._gridModel.DuplicateExists(newNumber, model.Row, model.Col);
        string newColor = NumberColor(dup);

        controller.FillNumber(newNumber, newColor);
        GameLog.Instance.WriteToLog($"(GridController.cs) Fill number {newNumber} in [{model.Row}, {model.Col}]");
    }

    // Handle undo button event
    public void UndoLastAction()
    {
        if (this._actionStack.Count == 0)
        {
            Debug.Log("GridController.cs: Nothing to undo");
            return;
        }

        // Retrieve previous action from stack
        UndoAction undo = this._actionStack.Pop();
        

        // Restore CellModel and CellController
        undo.cellController.Model.Num = undo.num;
        undo.cellController.FillNumber(undo.num, undo.numColor);

        GameLog.Instance.WriteToLog($"(GridController.cs) Undo Number {undo.num} in [{undo.cellController.Model.Row}, {undo.cellController.Model.Col}]");
    }

    public void RestartGame()
    {
        // Clear game status
        this._actionStack = new Stack<UndoAction>();
        this._timerController.RestartGame();
        this._hasGameCompleted = false;

        // Rebuild the grid
        this._cellModels = this._gridModel
            .GenerateGrid()
            .Cells;

        BuildGrid();

        GameLog.Instance.WriteToLog($"(GridController.cs) Game restarted.");
    }

    /// <summary>
    /// Save previous controller and number state in stack
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="model"></param>
    private void PushUndoState(CellController controller, CellModel model)
    {
        // Load & store previous information
        bool duplicateExist = this._gridModel.DuplicateExists(model.Num, model.Row, model.Col);
        string previousColor = NumberColor(duplicateExist);

        // Debug.Log($"[PUSH] At push time - num: {previousNum}, color: {previousColor}");
        this._actionStack.Push(new UndoAction { 
            cellController = controller,
            num = model.Num, 
            numColor = previousColor 
        });
    }

    // Determine color of the number
    private string NumberColor(bool duplicateExist)
    {
        if (duplicateExist) return "red";
        else return "blue";
    }
}
