using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameLog
{
    // Static instance accessible globally
    // Also only 1 instance is accessible to prevent unexpected result (ie. multiple duplicate of logs recorded)
    // TODO: Write to more logs (Error log, Stream read log etc)
    private static GameLog _instance;
    private StreamWriter sw;
    private GameLog()
    {
        string filePath = "./Assets/Resources/Log/Gamelog.txt";
        this.sw = new StreamWriter(filePath, true);
    }

    public static GameLog Instance
    {
        get {
            if (_instance == null)
            {
                _instance = new GameLog();
            }
            return _instance;
        }
    }

    // Write the log to Gamelog.txt
    public void WriteToLog(string msg)
    {
        using (this.sw)
        {
            this.sw.WriteLine(msg);
        }
    }
}
