using System.Collections.Generic;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    [SerializeField]
    private GameManager _mgr;

    private TimerModel _timerModel;
    private TimerContainerController[] _timerList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DisplayElapsedTime();
    }

    public TimerModel Model
    {
        get { return this._timerModel; }
    }

    public void Init()
    {
        this._timerModel = new TimerModel();
        this._timerList = new TimerContainerController[4];
        BuildTimerNumberControllerList();
    }

    /// <summary>
    /// Display the elapsed time on scene
    /// </summary>
    public void DisplayElapsedTime()
    {
        string time = this._timerModel.GetElapsedTime();
        int[] timeDigits = ConvertTimeStringToIntArray(time);

        for (int i = 0; i < timeDigits.Length; i++) 
        {
            this._timerList[i].DisplayDigit(timeDigits[i]);
        }
    }

    public void PauseGame()
    {
        this._timerModel.PauseTimer();
        _mgr.NotifyObservers();
    }

    public void ContinueGame()
    {
        this._timerModel.ContinueTimer();
        _mgr.NotifyObservers();
    }

    public bool IsPaused()
    {
        return this._timerModel.IsPaused;
    }

    public void RestartTimer()
    {
        this._timerModel.RestartTimer();
    }

    // Helper functions
    // Convert time from minute to second
    private int[] ConvertTimeStringToIntArray(string time)
    {
        int[] timeArray = new int[4];
        string[] strings = time.Split(":"); // split by minute and second

        timeArray[0] = strings[0][0] - '0';
        timeArray[1] = strings[0][1] - '0';
        timeArray[2] = strings[1][0] - '0';
        timeArray[3] = strings[1][1] - '0';

        return timeArray;
    }

    // Build up the timer container list
    private void BuildTimerNumberControllerList()
    {
        foreach (TimerContainerController ctr in FindObjectsByType<TimerContainerController>(FindObjectsSortMode.None))
        {
            ctr.Init(TimerNumberDatabase.Instance);
            // Debug.Log("Position: " + ctr.Position);
            this._timerList[ctr.Position] = ctr;
        }
    }
}
