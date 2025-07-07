using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class LeaderboardHistory
{
    private List<LeaderboardEntry> _entries;
    private List<string> _entriesText;

    private LeaderboardHistoryIO _io;
    
    public LeaderboardHistory()
    {
        _io = new LeaderboardHistoryIO();
    }

    public List<LeaderboardEntry> Entries
    {
        get { return _entries; }
        set { _entries = value; }
    }

    public List<string> EntriesText
    {
        get { return _entriesText; }
        set { _entriesText = value; }
    }

    public void SaveLeaderboard()
    {
        _io.Save(_entries);
    }

    public void LoadLeaderboard()
    {
        _entries = _io.Load();
    }

    public void AddRecord(string name, float time)
    {
        this._entries.Add(new LeaderboardEntry(name, time));
    }

    /// <summary>
    /// Construct the leaderboard text to be displayed on leaderboard from LeaderboardEntry
    /// </summary>
    /// <returns></returns>
    public LeaderboardHistory GenerateEntriesString()
    {
        this._entriesText = new List<string>();
        SortEntries();

        foreach (var entry in _entries)
        {
            string completionTime = Math.Round(entry.CompletionTime, 2).ToString();
            string entryStr = $"Player {entry.Name} | {completionTime} minutes" ;
            this._entriesText.Add(entryStr);
        }

        return this;
    }

    public LeaderboardHistory SortEntries()
    {
        _entries.Sort((a, b) => a.CompletionTime.CompareTo(b.CompletionTime));
        return this;
    }
}

