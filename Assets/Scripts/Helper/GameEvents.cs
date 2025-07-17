using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class GameEvents
{
    public static event Action<bool> OnGameReset;
    public static event Action OnGameComplete;
    public static event Action<bool> OnNewPuzzle;
    public static event Action<string, float> OnAddPlayerRecord;

    public static void ResetGame(bool newPuzzle)
    {
        OnGameReset?.Invoke(newPuzzle);
    }

    public static void GameComplete()
    {
        OnGameComplete?.Invoke();
    }

    public static void NewPuzzle(bool newPuzzle)
    {
        OnNewPuzzle?.Invoke(newPuzzle);
    }

    public static void AddPlayerRecord(string name, float completionTime)
    {
        OnAddPlayerRecord?.Invoke(name, completionTime);
    }
}
