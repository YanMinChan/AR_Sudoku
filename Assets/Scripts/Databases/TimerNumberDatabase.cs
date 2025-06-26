using UnityEngine;
using System.Collections.Generic;
public class TimerNumberDatabase : MonoBehaviour
{
    public static TimerNumberDatabase Instance { get; private set; }

    // Simple class to store number and their respective GameObject prefab
    [System.Serializable]
    public class NumberEntry
    {
        public int num;
        public GameObject prefab;
    }

    // Unity changable variable
    public List<NumberEntry> entries;

    // Instance variable
    private Dictionary<int, GameObject> _numberDict;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            BuildDictionary();
        }
    }

    private void BuildDictionary()
    {
        this._numberDict = new Dictionary<int, GameObject>();
        foreach (var entry in entries)
        {
            this._numberDict[entry.num] = entry.prefab;
        }
    }

    public GameObject GetTimerNumber(int num)
    {
        if (this._numberDict.TryGetValue(num, out GameObject prefab))
        {
            return prefab;
        }
        return null;
    }
}
