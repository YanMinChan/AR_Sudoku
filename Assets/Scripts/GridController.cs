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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Load puzzle data from CSV
        LoadData();

        // Manage other models and controllers
        AssignToCellModel();
        AssignNumberController();

        // Generate the grid view
        BuildGrid(init:true);
    }

    // Update is called once per frame
    void Update()
    {
        BuildGrid();
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

    // Assign grid to number controller
    private void AssignNumberController()
    {
        this.numberControllers = FindObjectsOfType<NumberController>().ToList();
        foreach (var cont in this.numberControllers)
        {
            cont.Initialize(this);
        }
    }

    public bool FillNumber(CellController controller, int number)
    {
        // Load the cell model assigned to the controller
        CellModel model = controller.Model;
        
        if (gridModel.numberIsValid(number, model.row, model.col)){
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
}
