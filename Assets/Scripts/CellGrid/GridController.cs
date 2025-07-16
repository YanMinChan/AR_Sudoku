using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// GridController class
/// Manages GridModel, and is a collection of CellControllers and NumberControllers
/// </summary>
public class GridController : MonoBehaviour, ITimerObserver
{
    [SerializeField]
    private GameManager _gameMgr;

    // Instance variables
    private GridModel _gridModel; 
    private CellController[,] _cellControllers;
    private List<NumberController> _numberControllers;

    private bool _isGamePaused;

    // Dependency Injection
    private ISoundEffectDatabase _sfxDatabase;
    private INumberDatabase _numberDatabase;
    private IToaster _toast;

    // Command manager
    private GameCommandManager _cmdMgr;

    // Get methods
    public GridModel Model { get { return _gridModel; } }
    public CellController[,] CellControllers { get { return _cellControllers; } }
    public List<NumberController> NumberControllers { get { return _numberControllers; } }

    void Awake() { }

    public void OnEnable()
    {
        UIEvents.OnUndoPressed += UndoLastAction;
        GameEvents.OnGameReset += ResetGrid;
        GameEvents.OnNewPuzzle += ResetGrid;
    }

    public void OnDisable()
    {
        UIEvents.OnUndoPressed -= UndoLastAction;
        GameEvents.OnGameReset -= ResetGrid;
        GameEvents.OnNewPuzzle -= ResetGrid;
    }

    // Constructor
    public void Init(ISoundEffectDatabase sfxDatabase, INumberDatabase numberDatabase, IToaster toast)
    {
        // Instantiate
        _gridModel = new GridModel();
        _cellControllers = new CellController[9, 9];
        _numberControllers = new List<NumberController>();

        _cmdMgr = new GameCommandManager();
        _isGamePaused = false;

        // Dependency injection
        this._sfxDatabase = sfxDatabase;
        this._numberDatabase = numberDatabase;
        _toast = toast;

        // Instantiate models and controllers
        this._gridModel.Init();
        CellControllersInit();
        NumberControllersInit();

        // Subscribe to game manager
        if (_gameMgr.Test == null) { Debug.Log("Is null"); }
        _gameMgr.AddTimerObserver(this);

        // Generate the grid view
        BuildGrid();
    }

    // Find all cell controllers object and assign them to their respective model
    private void CellControllersInit()
    {
        foreach (var controller in FindObjectsByType<CellController>(FindObjectsSortMode.None))
        {
            controller.Init(_sfxDatabase, _numberDatabase);

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
                this._cellControllers[i, j].FillCell("black", init:true, mute:true);
            }
        }
        // GameLog.Instance.WriteToLog("(GridController.cs) Grid gameObject built.");
    }

    enum ActionValidationResult 
    { 
        Success,
        GamePaused,
        NoCellSelected,
        UnchangableCell
    }

    private (ActionValidationResult, string) ValidateAction(CellController cellCtr)
    {
        if (_isGamePaused) return (ActionValidationResult.GamePaused, "Game is paused");
        if (cellCtr == null) return (ActionValidationResult.NoCellSelected, "No cell is selected");
        if (cellCtr.IsUnchangable) return (ActionValidationResult.UnchangableCell, "Cell is unchangeable");
        return (ActionValidationResult.Success, "Success");
    }

    // Fill number in the selected cell
    public void FillNumber(int number)
    {
        CellController cellCtr = CellController.currentlySelected;

        var (result, msg) = ValidateAction(cellCtr);
        if (result != ActionValidationResult.Success)
        {
            _toast.Show(msg);
            _sfxDatabase.PlayAudio(4);
            return;
        }

        var command = new FillNumberCommand(cellCtr, this._gridModel, number);
        _cmdMgr.ExecuteCommand(command);

        UpdateNumberBarVisibility();
        UpdateNumberColor();

        if (_gridModel.IsPuzzleFinished()) { _gameMgr.HasPuzzleFinished = true; }
    }

    // Handle undo button event
    public void UndoLastAction()
    {
        var (result, msg) = ValidateAction(CellController.currentlySelected);

        if (result == ActionValidationResult.GamePaused)
        {
            _toast.Show(msg);
            _sfxDatabase.PlayAudio(4);
            return;
        }

        if (!_cmdMgr.Undo()) { return; }

        UpdateNumberBarVisibility();
        UpdateNumberColor();
    }

    public void ResetGrid(bool newPuzzle = false)
    {
        // Clear game status
        _gridModel.ResetGrid(newPuzzle);
        _cmdMgr.ResetHistory();

        // Rebuild the grid
        foreach (var cell in _cellControllers)
        {
            cell.IsUnchangable = false;
        }
        BuildGrid();
        foreach (var n in _numberControllers)
        {
            n.gameObject.SetActive(true);
        }

        _toast.Show("Game Restarted!");
    }

    public void UpdateNumberBarVisibility()
    {
        for (int i = 1; i <= 9; i++)
        {
            NumberController numCtr = this._numberControllers.FirstOrDefault(n => n.Number == i);
            
            bool numUsed = this._gridModel.IsNumberFullyUsed(i);
            bool anyDuplicate = this._gridModel.AnyDuplicateExists(i);
            bool shouldHide = numUsed && !anyDuplicate;

            bool wasActive = numCtr.gameObject.activeSelf;

            numCtr.gameObject.SetActive(!shouldHide);

            // Only play the sfx when first time set disable and game not finished
            // Will play another sfx at game finish in GameManager
            if (wasActive && shouldHide && !_gridModel.IsPuzzleFinished()) { 
                _sfxDatabase.PlayAudio(5, 0.5f);
            }
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
                string color = duplicateExist ? "red" : "blue";
                this._cellControllers[r, c].FillCell(color, mute:true);
            }
        }
    }

    public void Invoke(bool IsGamePaused)
    {
        _isGamePaused = IsGamePaused;
    }
}
