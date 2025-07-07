using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public class LeaderboardHistoryIO
{
    private static string _filePath = AppPaths.LeaderboardHistoryFile;
    public LeaderboardHistoryIO(){}

    public List<LeaderboardEntry> Load()
    {
        if (!File.Exists(_filePath)) { return new List<LeaderboardEntry>(); }

        string json = File.ReadAllText(_filePath);
        return IOHelper.FromJson<List<LeaderboardEntry>>(json);
    }

    public void Save(List<LeaderboardEntry> entries) 
    {
        string json = IOHelper.ToJson<List<LeaderboardEntry>>(entries);
        File.WriteAllText(_filePath, json);
    }
}
