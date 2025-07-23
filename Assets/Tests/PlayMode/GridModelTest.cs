using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GridModelTest
{
    private GridModel _gridModel;

    [SetUp]
    public void SetUp()
    {
        // common Arrange
        _gridModel = new GridModel();
        _gridModel.GenerateGrid();

    }
    // A Test behaves as an ordinary method
    [Test]
    [TestCase(true, 1)]
    [TestCase(true, 3)]
    [TestCase(true, 4)]
    [TestCase(false, 5)]
    public void GridModel_numDup_Test_1(bool expected, int num)
    {
        // Set up content of some cells
        _gridModel.Cells[0, 0].Num = 1;
        _gridModel.Cells[0, 3].Num = 3;
        _gridModel.Cells[3, 1].Num = 4;

        Assert.AreEqual(expected, _gridModel.DuplicateExists(num, 0, 1)); // row 0 duplicates of 1
    }

    [Test]
    public void GridModel_numDup_Test_2()
    {
        // Set up content of some cells
        _gridModel.Cells[0, 0].Num = 1;
        _gridModel.Cells[1, 3].Num = 1;
        _gridModel.Cells[6, 1].Num = 1;

        _gridModel.Cells[1, 4].Num = 2;
        _gridModel.Cells[5, 1].Num = 2;

        _gridModel.Cells[1, 8].Num = 3;
        _gridModel.Cells[8, 1].Num = 3;

        Assert.AreEqual(true, _gridModel.DuplicateExists(1, 1, 1)); // 3 duplicate of 1 at row, col and subgrid
        Assert.AreEqual(true, _gridModel.DuplicateExists(2, 1, 1)); // 2 duplicate of 2 at row and col
        Assert.AreEqual(true, _gridModel.DuplicateExists(3, 1, 1)); // 2 duplicate of 3 at last row and last col
    }

    [Test]
    [TestCase(1, 3, 9)] // border case: number 9
    [TestCase(2, 5, 1)] // border case: number 1
    [TestCase(8, 2, 0)] // border case: number 0 (empty cell)
    [TestCase(7, 6, 3)]
    public void GridModel_numDup_Test_3(int row, int col, int num)
    {
        // Set up content of some cells
        _gridModel.Cells[row, col].Num = num;

        Assert.AreEqual(false, _gridModel.DuplicateExists(num, row, col)); // does not check duplicate on self
    }

    [Test]
    // all of these are empty cell
    [TestCase(1, 1)] 
    [TestCase(2, 1)]
    [TestCase(4, 5)]
    [TestCase(1, 5)]
    public void GridModel_numDup_Test_4(int row, int col)
    {
        // Set up content of some cells
        _gridModel.Cells[0, 0].Num = 1;
        _gridModel.Cells[1, 3].Num = 1;
        _gridModel.Cells[6, 1].Num = 1;

        _gridModel.Cells[1, 4].Num = 2;
        _gridModel.Cells[5, 1].Num = 2;

        _gridModel.Cells[1, 8].Num = 3;
        _gridModel.Cells[8, 1].Num = 3;

        Assert.AreEqual(false, _gridModel.DuplicateExists(0, row, col)); // does not check duplicate on number 0 (empty cell)
    }

    // Assumption: there will be no duplicates in solution
    [Test]
    [TestCase(new int[] { 1, 4, 3, 2, 5, 7, 9, 8, 6 }, true)] // correct answer
    [TestCase(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, false)] // incorrect ans but no duplicate
    [TestCase(new int[] { 1, 0, 3, 0, 5, 0, 0, 8, 6 }, false)] // correct answer but not completed
    [TestCase(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 }, false)] // empty subgrid
    public void GridModel_gameFinished_Test_1(int[] num, bool result)
    {
        // Set up content of some cells
        _gridModel.Cells[0, 0].Sol = 1;
        _gridModel.Cells[0, 1].Sol = 4;
        _gridModel.Cells[0, 2].Sol = 3;
        _gridModel.Cells[1, 0].Sol = 2;
        _gridModel.Cells[1, 1].Sol = 5;
        _gridModel.Cells[1, 2].Sol = 7;
        _gridModel.Cells[2, 0].Sol = 9;
        _gridModel.Cells[2, 1].Sol = 8;
        _gridModel.Cells[2, 2].Sol = 6;

        _gridModel.Cells[0, 0].Num = num[0];
        _gridModel.Cells[0, 1].Num = num[1];
        _gridModel.Cells[0, 2].Num = num[2];
        _gridModel.Cells[1, 0].Num = num[3];
        _gridModel.Cells[1, 1].Num = num[4];
        _gridModel.Cells[1, 2].Num = num[5];
        _gridModel.Cells[2, 0].Num = num[6];
        _gridModel.Cells[2, 1].Num = num[7];
        _gridModel.Cells[2, 2].Num = num[8];

        Assert.AreEqual(result, _gridModel.IsPuzzleFinished());
    }

    // Test for generating empty grid
    [Test]
    public void GridModel_GenerateGrid_Test_1()
    {
        GridModel gridModelTest1 = new GridModel();
        GridModel gridModelTest2 = new GridModel();
        gridModelTest1.GenerateGrid(new int[81], new int[81]);
        gridModelTest2.GenerateGrid();

        // Check all the cells are empty
        for (int r = 0; r < gridModelTest1.Cells.GetLength(0); r++)
        {
            for (int c = 0; c < gridModelTest1.Cells.GetLength(1); c++)
            {
                if (gridModelTest1.Cells[r, c].Num != gridModelTest2.Cells[r, c].Num || gridModelTest1.Cells[r, c].Num != 0 || gridModelTest2.Cells[r, c].Num != 0) 
                {
                    Assert.Fail($"Error: cell[{r}, {c}]. All cells number should equal and empty!");
                }

                if (gridModelTest1.Cells[r, c].Sol != gridModelTest2.Cells[r, c].Sol || gridModelTest1.Cells[r, c].Sol != 0 || gridModelTest2.Cells[r, c].Sol != 0)
                {
                    Assert.Fail($"Error: cell[{r}, {c}]. All cells solution should equal and empty!");
                }
            }
        }
    }

    // Test input column 0
    [Test]
    public void GridModel_GenerateGrid_Test_2()
    {
        // Construct the model
        GridModel gridModelTest3 = new GridModel();
        
        // Populate first column of the num and sol array
        int[] num = new int[81];
        int[] sol = new int[81];

        for (int i = 0; i < 9; i++)
        {
            num[i * 9] = i;
            sol[i * 9] = i;
        }

        // Generate the grid
        gridModelTest3.GenerateGrid(num, sol);

        // Check all the cells match the input array
        for (int r = 0; r < gridModelTest3.Cells.GetLength(0); r++)
        {
            if (gridModelTest3.Cells[r, 0].Num != r)
            {
                Assert.Fail($"Error: cell[{r}, 0]. Number in grid does not match input array!");
            }
            if (gridModelTest3.Cells[r, 0].Sol != r)
            {
                Assert.Fail($"Error: cell[{r}, 0]. Solution in grid does not match input array!");
            }

        }
    }

    // Test input row 0
    [Test]
    public void GridModel_GenerateGrid_Test_3()
    {
        // Construct the model
        GridModel gridModelTest3 = new GridModel();

        // Populate first row of the num and sol array
        int[] num = new int[81];
        int[] sol = new int[81];

        for (int i = 0; i < 9; i++)
        {
            num[i] = i;
            sol[i] = i;
        }

        // Generate the grid
        gridModelTest3.GenerateGrid(num, sol);

        // Check all the cells match the input array
        for (int c = 0; c < gridModelTest3.Cells.GetLength(0); c++)
        {
            if (gridModelTest3.Cells[0, c].Num != c)
            {
                Assert.Fail($"Error: cell[{c}, 0]. Number in grid does not match input array!");
            }
            if (gridModelTest3.Cells[0, c].Sol != c)
            {
                Assert.Fail($"Error: cell[{c}, 0]. Solution in grid does not match input array!");
            }

        }
    }

    // Test input subgrid 1
    [Test]
    public void GridModel_GenerateGrid_Test_4()
    {
        // Construct the model
        GridModel gridModelTest4 = new GridModel();

        // Populate first subgrid of the num and sol array
        int[] num = new int[81];
        int[] sol = new int[81];

        int val = 1;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                num[i * 9 + j] = val;
                sol[i * 9 + j] = val;
                val++;
            }

        }

        // Generate the grid
        gridModelTest4.GenerateGrid(num, sol);

        // Check all the cells match the input array
        int checkVal = 1;
        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                if (gridModelTest4.Cells[r, c].Num != checkVal)
                {
                    Assert.Fail($"Error: cell[{r}, {c}]. Number in grid does not match input array");
                }
                if (gridModelTest4.Cells[r, c].Sol != checkVal)
                {
                    Assert.Fail($"Error: cell[{r}, {c}]. Solution in grid does not match input array");
                }
                checkVal++;
            }
        }
    }

    // Test returned puzzle and solution is correct (first puzzle)
    [Test]
    public void GridModel_puzzleSelector_Test_1()
    {
        _gridModel.SelectPuzzle(0);
        int[] puz = _gridModel.Puz;
        int[] sol = _gridModel.Sol;
        int[] expectedPuz = {0,0,4,3,0,0,2,0,9,0,0,5,0,0,9,0,0,1,0,7,0,0,6,0,0,4,3,0,0,6,0,0,2,0,8,7,1,9,0,0,0,7,4,0,0,0,5,0,0,8,3,0,0,0,6,0,0,0,0,0,1,0,5,0,0,3,5,0,8,6,9,0,0,4,2,9,1,0,3,0,0};
        int[] expectedSol = {8,6,4,3,7,1,2,5,9,3,2,5,8,4,9,7,6,1,9,7,1,2,6,5,8,4,3,4,3,6,1,9,2,5,8,7,1,9,8,6,5,7,4,3,2,2,5,7,4,8,3,9,1,6,6,8,9,7,3,4,1,2,5,7,1,3,5,2,8,6,9,4,5,4,2,9,1,6,3,7,8};

        Assert.AreEqual(expectedPuz, puz);
        Assert.AreEqual(expectedSol, sol);
    }

    // Test returned puzzle and solution is not null
    [Test]
    public void GridModel_puzzleSelector_Test_2()
    {
        _gridModel.SelectPuzzle();

        Assert.IsNotNull(_gridModel.Puz);
        Assert.IsNotNull(_gridModel.Sol);
    }
}
