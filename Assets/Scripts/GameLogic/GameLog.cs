using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GameLog
{
    // Static instance accessible globally
    // Also only 1 instance is accessible to prevent unexpected result (ie. multiple duplicate of logs recorded)
    // TODO: Write to more logs (Error log, Stream read log etc)
    private static GameLog _instance;
    private StreamWriter _sw;
    private string _dirPath = "./Assets/Resources/Log/";
    private GameLog()
    {
    }

    public static GameLog Instance
    {
        get {
            if (_instance == null)
            {
                _instance = new GameLog();
                _instance.Initialise();
            }
            return _instance;
        }
    }

    private void Initialise()
    {
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        // string _filePath = Path.Combine(_dirPath, $"GameLog_{timestamp}.txt"); // For more GameLog with diff timestamp
        string _filePath = Path.Combine(_dirPath, $"GameLog.txt");
        this._sw = new StreamWriter(_filePath, false);
    }

    // Write the log to Gamelog.txt
    public void WriteToLog(string msg)
    { 
        if (this._sw == null) return; 
        // Timestamp
        string timeNow = DateTime.Now.ToString("HH:mm:ss:ff");

        this._sw.WriteLine($"[{timeNow}] {msg}");
        this._sw.Flush();
    }

    public void CloseLogger()
    {
        if (this._sw != null)
        {
            this._sw.Close();
            this._sw = null;
        }
    }
}
