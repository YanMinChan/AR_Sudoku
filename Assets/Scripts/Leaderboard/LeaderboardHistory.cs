using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

public class LeaderboardHistory
{
    private List<LeaderboardEntry> _entries;
    private static string _leaderboardPath = "./Assets/Resources/Leaderboard/history.json";
    private List<string> _entriesText;

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
        // GenerateSomeEntries();
        string json = JsonConvert.SerializeObject(_entries);
        File.WriteAllText(_leaderboardPath, json);
    }

    public void LoadLeaderboard()
    {
        if (!File.Exists(_leaderboardPath))
        {
            this._entries = new List<LeaderboardEntry>();
        }
        else
        {
            string json = File.ReadAllText(_leaderboardPath);
            this._entries = JsonConvert.DeserializeObject<List<LeaderboardEntry>>(json);
        }
    }

    public void AddRecord(string name, float time)
    {
        this._entries.Add(new LeaderboardEntry(name, time));
    }

    public LeaderboardHistory GenerateEntriesString()
    {
        this._entriesText = new List<string>();
        foreach (var entry in _entries)
        {
            string completionTime = Math.Round(entry.CompletionTime, 2).ToString();
            string entryStr = $"Player {entry.Name} | {completionTime} minutes" ;
            this._entriesText.Add(entryStr);
        }
        return this;
    }

    private void GenerateSomeEntries()
    {
        LeaderboardEntry entry1 = new LeaderboardEntry("hehehe", 2.18f);
        LeaderboardEntry entry2 = new LeaderboardEntry("hohoho", 3.18f);
        LeaderboardEntry entry3 = new LeaderboardEntry("hihihi", 4.18f);

        this._entries = new List<LeaderboardEntry>();
        this._entries.Add(entry1);
        this._entries.Add(entry2);
        this._entries.Add(entry3);
    }
}

