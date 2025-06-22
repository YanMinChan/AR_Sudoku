using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// Fill the grid with the puzzle
/// </summary>
public class GridController : MonoBehaviour
{
    GridModel gridModel; 
    CellModel[,] cellModels;
    CellController[,] cellControllers;
    List<NumberController> numberControllers;
    Stack<UndoAction> actionStack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Load puzzle data from CSV
        LoadData();

        // Manage other models and controllers
        AssignToCellModel();
        AssignNumberController();

        // Instantiate the action stack
        this.actionStack = new Stack<UndoAction>();

        // Generate the grid view
        BuildGrid(init:true);
    }

    // Update is called once per frame
    void Update()
    {
        //BuildGrid();
        // Check if the game is finished
        if (this.gridModel.gameFinished())
        {
            // TODO: Change to on scene feedback
            Debug.Log("YOU WINNNNN");
        }
    }

    // Load data from csv
    private void LoadData()
    {
        // Load grid data from model
        this.gridModel = new GridModel();
        string filePath = "./Assets/Resources/sudoku.csv"; // path of the dataset
        (int[] puz, int[] sol) = this.gridModel.puzzleSelector(filePath);
        this.cellModels = this.gridModel.GenerateGrid(puz, sol);

        if (cellModels != null)
            Debug.Log("Cell loaded: " + cellModels.Length);
        else
            Debug.Log("Error, cells not loaded");
    }

    // Find all cell controllers object and assign them to their respective model
    private void AssignToCellModel()
    {
        this.cellControllers = new CellController[9, 9];
        foreach (var controller in FindObjectsOfType<CellController>())
        {
            int r = controller.editorRow;
            int c = controller.editorCol;
            
            // Connect the cell model and controller
            try
            {
                controller.Model = cellModels[r-1, c-1];
                this.cellControllers[r-1, c-1] = controller;
            }
            catch { Debug.LogWarning((r-1) + " " + (c-1)); }
        }
    }

    // Assign grid to number controller
    private void AssignNumberController()
    {
        this.numberControllers = FindObjectsOfType<NumberController>().ToList();
        foreach (var cont in this.numberControllers)
        {
            cont.Initialize(this);
        }
    }

    // Build grid
    private void BuildGrid(bool init=false)
    {
        // Update cell controller for change in grid
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                int numbers = cellModels[i, j].num;
                if (numbers != 0)
                {   
                    if (init) 
                    {
                        this.cellControllers[i, j].FillNumber(numbers, "black", init);
                    } 
                    else
                    {
                        this.cellControllers[i, j].FillNumber(numbers, "blue");
                    }
                }
            }
        }
    }

    // Fill number in the selected cell
    public void FillNumber(CellController controller, int currNumber)
    {
        // Load the cell model assigned to the controller
        CellModel model = controller.Model;

        // Load previous data
        Func<int, string> checkColor = numDup => numDup == 0 ? "blue" : "red";
        int previousNum = model.num;
        int previousNumDup = gridModel.numberOfDuplicate(previousNum, model.row, model.col);
        string previousColor = checkColor(previousNumDup);

        // Store the previous information
        Debug.Log($"[PUSH] At push time - num: {previousNum}, color: {previousColor}");
        this.actionStack.Push(new UndoAction { cellController = controller, num = previousNum, numColor = previousColor});

        // Check for duplicate number
        int numDup = gridModel.numberOfDuplicate(currNumber, model.row, model.col);
        string currColor = checkColor(numDup);

        // Update the model and controller
        model.num = currNumber;
        controller.FillNumber(currNumber, currColor);
    }

    public void UndoLastAction()
    {
        SoundEffectDatabase.Instance.PlayAudio(1);
        if (actionStack.Count == 0)
        {
            Debug.Log("GridController.cs: Nothing to undo");
            return;
        }
        UndoAction undo = this.actionStack.Pop();
        Debug.Log($"[POP] From stack - num: {undo.num}, color: {undo.numColor}");
        int previousNum = undo.num;
        string previousColor = undo.numColor;

        // Restore model number
        undo.cellController.Model.num = previousNum;

        // Restore visual
        undo.cellController.FillNumber(previousNum, previousColor);
    }

}
