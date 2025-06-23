using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    private TimerModel _timerModel;
    private List<TimerNumberController> _timerList;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this._timerModel = new TimerModel();
        this._timerList = new List<TimerNumberController>();
        BuildTimerNumberControllerList();
    }

    // Update is called once per frame
    void Update()
    {
        //string time = this._timerModel.GetElapsedTime();
        //Debug.Log(time);
        DisplayElapsedTime();
    }

    public void DisplayElapsedTime()
    {
        string time = this._timerModel.GetElapsedTime();
        int[] timeDigits = ConvertTimeStringToIntArray(time);

        for (int i = 0; i < timeDigits.Length; i++) 
        {
            GameObject timerPrefab = TimerNumberDatabase.Instance.GetTimerNumber(timeDigits[i]);
            this._timerList[i].TimerNumber = timerPrefab;
            Debug.Log(timeDigits[i]);
        }
        //Debug.Log(string.Join(" ", timeDigits));
    }

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

    private void BuildTimerNumberControllerList()
    {
        foreach (TimerNumberController ctr in FindObjectsByType<TimerNumberController>(FindObjectsSortMode.None))
        {
            this._timerList.Add(ctr);
        }
    }
}
