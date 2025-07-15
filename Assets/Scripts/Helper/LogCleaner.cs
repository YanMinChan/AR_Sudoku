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
    public static void CleanOldLog(int num = 5)
    {
        // Get all logfiles
        string dir = Path.GetDirectoryName(_logPath);
        var files = Directory.GetFiles(dir).Select(f=> new FileInfo(f)).OrderBy(f=>f.CreationTime);

        // fill not clean log if less than num present
        if (files.Count() < num) return;

        // Delete the log files
        foreach (var file in files)
        {
            string normaliseFileName = _logPath.Replace('/', '\\');
            if (file.FullName == normaliseFileName) continue;
            else
            {
                File.Delete(file.FullName);
                GameLogger.Instance.WriteToLog($"Deleted log: {file.FullName}");
            }
        }
    }
}
