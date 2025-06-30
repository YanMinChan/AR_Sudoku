using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LeaderboardEntry
{
    private string _name;
    private string _completionTime;

    public LeaderboardEntry(string name, string completionTime)
    {
        this._name = name;
        this._completionTime = completionTime;
    }

    public string Name
    {
        get { return _name; }
    }

    public string CompletionTime
    {
        get { return _completionTime; }
    }
}