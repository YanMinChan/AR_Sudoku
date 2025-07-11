using System.Collections.Generic;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    private TimerModel _timerModel;
    private TimerContainerController[] _timerList;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this._timerModel = new TimerModel();
        this._timerList = new TimerContainerController[4];
        BuildTimerNumberControllerList();
    }

    // Update is called once per frame
    void Update()
    {
        // Loading elapsed time from model
        //string time = this._timerModel.GetElapsedTime();
        //Debug.Log(time);

        DisplayElapsedTime();
    }

    public TimerModel Model
    {
        get { return this._timerModel; }
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
            // GameObject timerPrefab = TimerNumberDatabase.Instance.GetTimerNumber(timeDigits[i]);
            this._timerList[i].DisplayDigit(timeDigits[i]);
            // Debug.Log(timeDigits[i]);
        }
        //Debug.Log(string.Join(" ", timeDigits));
    }

    public void PauseGame()
    {
        this._timerModel.PauseTimer();
    }

    public void ContinueGame()
    {
        this._timerModel.ContinueTimer();
    }

    public bool IsPaused()
    {
        return this.Model.IsPaused;
    }

    public void RestartGame()
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
            // Debug.Log("Position: " + ctr.Position);
            this._timerList[ctr.Position] = ctr;
        }
    }
}
