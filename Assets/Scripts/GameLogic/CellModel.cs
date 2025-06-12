using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CellModel
{
    public int num, sol, sgrid, row, col; // attributes of the cell

    public CellModel(int num, int sol, int sgrid, int row, int col)
    {
        this.num = num;
        this.sol = sol;
        this.sgrid = sgrid;
        this.row = row;
        this.col = col;
    }
}
