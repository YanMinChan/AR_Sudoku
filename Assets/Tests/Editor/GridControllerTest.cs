using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Unity.VisualScripting;

public class GridControllerTest
{
    private GridController _controller;
    private GameObject _grid;

    [SetUp]
    public void SetUp()
    {
        _grid = new GameObject("testGrid");

        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++) 
            {
                var go = new GameObject($"cell {r}{c}").AddComponent<CellController>();
                go.editorRow = r + 1;
                go.editorCol = c + 1;
                // go.Model = _controller.GetGridModel().Cells[r, c];
            }
        }
        _controller = _grid.AddComponent<GridController>();
        _controller.Init();
    }

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
}
