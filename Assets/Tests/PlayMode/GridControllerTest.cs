using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GridControllerTest
{
    private GridController _controller;
    private GameObject _grid;

    [SetUp]
    public void SetUp()
    {
        // Create Grid and Cell GameObjects
        _grid = new GameObject("testGrid");

        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++) 
            {
                var go = new GameObject($"cell {r}{c}").AddComponent<CellController>();
                go.editorRow = r + 1;
                go.editorCol = c + 1;
            }
        }

        // Create mock database
        var mockNumberDB = new MockNumberDatabase();
        // Add script and init
        _controller = _grid.AddComponent<GridController>();
        _controller.Init(SoundEffectDatabase.Instance, mockNumberDB, Toaster.Instance);
    }

    /// <summary>
    /// Check if the CellControllers's CellModel always equal the GridModel's CellModels
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator GridController_CellControllerInit_Test_1()
    {
        yield return null; // Wait for init

        for (int r = 0; r < _controller.GetCellControllers().GetLength(1); r++)
        {
            for (int c = 0; c < _controller.GetCellControllers().GetLength(1); c++)
            {
                Assert.AreEqual(
                _controller.GetCellControllers()[r, c].Model,
                _controller.GetGridModel().Cells[r, c]
                );
            }
        }
    }

    /// <summary>
    /// Check GameObject of number 1 to 9 is stored
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator GridController_NumberControllersInit_Test_1()
    {
        yield return null;

        for (int i = 1; i <= 9; i++)
        {
            Assert.AreEqual(i, _controller.GetNumberControllers()[i - 1].Number);
        }
    }

    [Test]
    public void GridController_ActionValidationResult_Test()
    {

    }
}
