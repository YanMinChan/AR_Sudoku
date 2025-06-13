using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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
        BuildGrid();
    }

    // Load data from csv
    private void LoadData()
    {
        // Load grid data from model
        this.gridModel = new GridModel();
        string filePath = "./Assets/Resources/sudoku.csv"; // path of the dataset
        this.cellModels = this.gridModel.GenerateGrid(filePath);

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
                    this.cellControllers[i, j].FillNumber(numbers);
                    if (init) 
                    { 
                        this.cellControllers[i, j].isDefaultCell = true; 
                    }
                }
            }
        }
    }

    public bool FillNumber(CellController controller, int number)
    {
        // Load the cell model assigned to the controller
        CellModel model = controller.Model;
        
        if (gridModel.numberIsValid(number, model.row, model.col)){
            // Store the previous information
            actionStack.Push(new UndoAction { cellController=controller, num=model.num, numberPrefab=controller.numberPrefab });
            // Update the model
            model.num = number;
            // Update the cell controller
            controller.FillNumber(number);

            return true;
        } 
        else
        {
            // Change it to fill the number with red highlight
            Debug.Log("Number not valid AAAA");
            return false;
        }
    }

    public void UndoLastAction()
    {
        if (actionStack.Count == 0)
        {
            Debug.Log("Nothing to undo");
            return;
        }

        UndoAction undo = actionStack.Pop();

        // Restore model number
        undo.cellController.Model.num = undo.num;

        // Restore visual
        undo.cellController.FillNumber(undo.num);
    }

}
