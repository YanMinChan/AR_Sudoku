using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CellModel
{
    int num;
    int sol;
    int sgrid;
    int row;
    int col;
    public CellModel(int num, int sol, int sgrid, int row, int col)
    {
        this.num = num;
        this.sol = sol;
        this.sgrid = sgrid;
        this.row = row;
        this.col = col;
    }

    public int Number
    {
        get { return num; }
        set { num = value; }
    }

    public int Solution
    {
        get { return sol; }
    }

    public int Row
    {
        get { return row; }
    }

    public int Col
    {
        get { return col; }
    }
}
