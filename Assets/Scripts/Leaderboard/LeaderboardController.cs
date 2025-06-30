using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class LeaderboardController : MonoBehaviour
{
    private List<string> _texts;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetLeaderboardTMP()
    {
        // Get the records TMP displays
        List<TMP_Text> records = GameObject.FindGameObjectsWithTag("LbRecord")
            .SelectMany(obj =>obj.GetComponentsInChildren<TMP_Text>(true))
            .ToList();

        int i = 0;
        foreach (var record in records)
        {
            record.text = $"Change me {i}";
            i++;
        }
    }
}
