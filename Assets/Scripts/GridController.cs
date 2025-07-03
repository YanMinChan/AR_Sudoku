using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Unity.VisualScripting;

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
        if (this._gridModel.GameFinished())
        {
            // TODO: Change to on scene feedback
            Debug.Log("YOU WINNNNN");
        }
    }

    // Load data from csv to cell model
    private void InstantiateCellModels()
    {
        // Instantiate cell model
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "sudoku.csv");
        //string filePath = "./Assets/Resources/sudoku.csv"; // path of the dataset
        this._cellModels = this._gridModel.SelectPuzzle(filePath)
            .GenerateGrid()
            .Cells;

        if (this._cellModels != null)
        {
            string msg = "Cell loaded: " + this._cellModels.Length;
            Debug.Log(msg);
            //GameLog.Instance.WriteToLog(msg);
        }
        else
            Debug.Log("Error, cells not loaded");
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
                int numbers = this._cellModels[i, j].num;
                this._cellControllers[i, j].FillNumber(numbers, "black", init:true);
            }
        }
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
        model.num = newNumber;

        // Determine number and update view
        bool dup = this._gridModel.DuplicateExists(newNumber, model.row, model.col);
        string newColor = NumberColor(dup);

        controller.FillNumber(newNumber, newColor);
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
        // Debug.Log($"[POP] From stack - num: {undo.num}, color: {undo.numColor}");

        // Restore CellModel and CellController
        undo.cellController.Model.num = undo.num;
        undo.cellController.FillNumber(undo.num, undo.numColor);
    }

    public void RestartGame()
    {
        // Clear the stack & timer
        this._actionStack = new Stack<UndoAction>();
        this._timerController.RestartGame();

        // Rebuild the grid
        this._cellModels = this._gridModel
            .GenerateGrid()
            .Cells;

        Debug.Log(this._cellModels[0, 0].num);
        BuildGrid();
    }

    /// <summary>
    /// Save previous controller and number state in stack
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="model"></param>
    private void PushUndoState(CellController controller, CellModel model)
    {
        // Load & store previous information
        bool duplicateExist = this._gridModel.DuplicateExists(model.num, model.row, model.col);
        string previousColor = NumberColor(duplicateExist);

        // Debug.Log($"[PUSH] At push time - num: {previousNum}, color: {previousColor}");
        this._actionStack.Push(new UndoAction { 
            cellController = controller,
            num = model.num, 
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
