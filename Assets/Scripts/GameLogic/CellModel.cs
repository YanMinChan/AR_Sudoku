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
    public CellModel(int num, int sol, int sgrid)
    {
        this.num = num;
        this.sol = sol;
        this.sgrid = sgrid;
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
}
