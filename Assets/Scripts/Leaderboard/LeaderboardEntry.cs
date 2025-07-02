using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LeaderboardEntry
{
    private string _name;
    private float _completionTime;

    public LeaderboardEntry(string name, float completionTime)
    {
        this._name = name;
        this._completionTime = completionTime;
    }

    public string Name
    {
        get { return _name; }
    }

    public float CompletionTime
    {
        get { return _completionTime; }
    }
}