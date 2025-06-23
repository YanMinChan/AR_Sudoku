using System.Diagnostics;
using UnityEngine;
using System;

public class TimerModel
{
    // Instance variable
    private Stopwatch _timer;

    public TimerModel()
    {
        this._timer = new Stopwatch();
        this._timer.Start();
    }

    public string GetElapsedTime()
    {
        TimeSpan ts = this._timer.Elapsed;
        string elapsedTime = String.Format("{0:00}:{1:00}",
            ts.Minutes, ts.Seconds);
        return elapsedTime;
    }
}
