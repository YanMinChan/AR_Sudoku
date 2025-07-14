using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MockNumberDatabase: INumberDatabase
{
    private Dictionary<int, GameObject> _prefabs = new();

    public MockNumberDatabase()
    {
        for (int i = 1; i <= 9; i++)
        {
            var go = new GameObject($"Number_{i}");
            go.tag = "NumberBar";

            var controller = go.AddComponent<NumberController>();
            controller.Number = i;

            _prefabs[i] = go;
        }
    }

    public GameObject GetNumber(int number)
    {
        if (!_prefabs.ContainsKey(number)) return null;
        return _prefabs[number];
    }
}

