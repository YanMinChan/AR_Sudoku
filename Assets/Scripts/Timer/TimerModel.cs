using System.Diagnostics;
using UnityEngine;
using System;

public class TimerModel
{
    // Might need singleton?
    // Instance variable
    private Stopwatch _timer;
    private bool _isPaused;

    public TimerModel()
    {
        this._timer = new Stopwatch();
        this._timer.Start();
        _isPaused = false;
    }

    public bool IsPaused
    {
        get { return this._isPaused; }
        set { this._isPaused = value; }
    }

    public string GetElapsedTime()
    {
        TimeSpan ts = this._timer.Elapsed;
        string elapsedTime = String.Format("{0:00}:{1:00}",
            ts.Minutes, ts.Seconds);
        return elapsedTime;
    }

    public float GetElapsedTimeFloat()
    {
        TimeSpan ts = this._timer.Elapsed;
        float minutes = (float)ts.TotalMinutes;
        return minutes;
    }

    public void PauseTimer()
    {
        this._timer.Stop();
        IsPaused = true;
    }

    public void ContinueTimer()
    {
        this._timer.Start(); // Start() also resumes previous timer
        IsPaused = false;
    }

    public void RestartTimer()
    {
        this._timer.Restart();
        IsPaused = false;
    }
}
