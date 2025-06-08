using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CellLogic
{
    int num;
    int sol;
    int sgrid;
    public CellLogic(int num, int sol, int sgrid)
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

}
