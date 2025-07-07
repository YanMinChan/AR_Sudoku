using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class AppPaths
{
    public static readonly string persistantRoot = Application.persistentDataPath;
    public static readonly string streamingRoot = Application.streamingAssetsPath;

    public static string Logs = Path.Combine(persistantRoot, "Logs");
    public static string GameLogFile = Path.Combine(Logs, $"GameLog_{DateTime.Now: yyyyMMdd_HHmmss}.txt");

    public static string LeaderboardHistoryFile = Path.Combine(persistantRoot, "history.json");

    public static string PuzzleFile = Path.Combine(streamingRoot, "sudoku.csv");
}
