using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;

// Command pattern: Fill number in cell and validate the grid's status (duplicate, digit usage of each number)
public class FillNumberCommand: IGameCommand
{
    private readonly CellController _ctr;
    private readonly GridModel _gridModel;
    private readonly int _num;

    private int _oldNum;
    private string _oldColor;
    public FillNumberCommand(CellController ctr, GridModel gridModel, int num)
    {
        _ctr = ctr;
        _gridModel = gridModel;
        _num = num;

        _oldNum = _ctr.Model.Num;
        bool dup = gridModel.DuplicateExists(_oldNum, _ctr.Model.Row, _ctr.Model.Col);
        _oldColor = dup ? "red" : "blue";
    }

    // Set up number and color in cell and validate the grid
    public void Execute() 
    {
        // First check for duplicate
        bool duplicate = _gridModel.DuplicateExists(_num, _ctr.Model.Row, _ctr.Model.Col);
        string color = duplicate ? "red" : "blue";

        // Update cell 
        _ctr.SetNumber(_num).FillCell(color);
        _gridModel.CalculateDigitUsage();
    }

    // Undo previous action
    public void Undo() 
    {
        _ctr.SetNumber(_oldNum).FillCell(_oldColor);
        _gridModel.CalculateDigitUsage();
    }
}
