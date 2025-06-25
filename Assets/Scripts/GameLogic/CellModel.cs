using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CellModel
{
    private int _num, _sol, _row, _col; // attributes of the cell

    public CellModel(int num, int sol, int row, int col)
    {
        this._num = num;
        this._sol = sol;
        this._row = row;
        this._col = col;
    }

    public int Num
    {
        get { return this._num; }
        set { this._num = value; }
    }

    public int Sol
    { 
        get { return this._sol; }
        set { this._sol = value; }
    }

    public int Row
    { 
        get { return this._row; } 
        set { this._row = value; }
    }

    public int Col
    { 
        get { return this._col; } 
        set { this._col = value; }
    }

}
