using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class LogCleaner
{
    private static string _logPath = AppPaths.GameLogFile;
    public static void CleanOldLog(int num = 20)
    {
        // Get all logfiles
        string dir = Path.GetDirectoryName(_logPath);
        var files = Directory.GetFiles(dir);

        if (files.Count() <= num) return;

        var filesToDelete = files.Select(f=> new FileInfo(f)).OrderBy(f=>f.CreationTime).Take(files.Length - num);

        // fill not clean log if less than num present
        

        // Delete the log files
        foreach (var file in filesToDelete)
        {
            File.Delete(file.FullName);
            GameLogger.Instance.WriteToLog($"Deleted log: {file.FullName}");
        }
    }
}
