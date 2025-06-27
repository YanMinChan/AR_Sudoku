using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

public class LeaderboardHistory
{
    private List<LeaderboardEntry> entries;
    private static string _leaderboardPath = "./Assets/Resources/Leaderboard";

    public void SaveLeaderboard()
    {
        string json = JsonConvert.SerializeObject(entries);
        File.WriteAllText(_leaderboardPath, json);

    }

    public List<LeaderboardEntry> LoadLeaderboard()
    {
        if (!File.Exists(_leaderboardPath))
        {
            return new List<LeaderboardEntry>();
        }

        string json = File.ReadAllText(_leaderboardPath);
        return JsonConvert.DeserializeObject<List<LeaderboardEntry>>(json);
    }

}

