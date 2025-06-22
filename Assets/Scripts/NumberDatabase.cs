using UnityEngine;
using System.Collections.Generic;
public class NumberDatabase : MonoBehaviour
{
    public static NumberDatabase Instance { get; private set; }

    [System.Serializable]
    public class NumberEntry
    {
        public int num;
        public GameObject prefab;
    }

    public List<NumberEntry> entries;

    private Dictionary<int, GameObject> numberDict;

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

    void BuildDictionary()
    {
        numberDict = new Dictionary<int, GameObject>();
        foreach (var entry in entries)
        {
            numberDict[entry.num] = entry.prefab;
        }
    }

    public GameObject GetNumber(int num)
    {
        if (numberDict.TryGetValue(num, out GameObject prefab))
        {
            return prefab;
        }
        return null;
    }
}
