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
    private static string _leaderboardPath = "./Assets/Resources/Leaderboard/";

    public List<LeaderboardEntry> Entries
    {
        get { return _entries; }
        set { _entries = value; }
    }

    public void SaveLeaderboard()
    {
        // GenerateSomeEntries();
        string json = JsonConvert.SerializeObject(_entries);
        File.WriteAllText(_leaderboardPath + "/history.json", json);
    }

    public List<LeaderboardEntry> LoadLeaderboard()
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
        return this._entries;
    }

    private void GenerateSomeEntries()
    {
        LeaderboardEntry entry1 = new LeaderboardEntry("hehehe", "02:18");
        LeaderboardEntry entry2 = new LeaderboardEntry("hohoho", "03:18");
        LeaderboardEntry entry3 = new LeaderboardEntry("hihihi", "04:18");

        this._entries = new List<LeaderboardEntry>();
        this._entries.Add(entry1);
        this._entries.Add(entry2);
        this._entries.Add(entry3);
    }
}

