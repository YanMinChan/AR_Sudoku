using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CellModel
{
    public int num, sol, row, col; // attributes of the cell

    public CellModel(int num, int sol, int row, int col)
    {
        this.num = num;
        this.sol = sol;
        this.row = row;
        this.col = col;
    }
}
