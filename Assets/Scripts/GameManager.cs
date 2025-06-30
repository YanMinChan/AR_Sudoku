using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Unity changable variables
    private GridController _gridController;
    private TimerController _timerController;
    private LeaderboardController _leaderboardController;

    // Instance variables
    private bool _hasGameCompleted = false;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        this._gridController = GameObject.Find("Grid").GetComponent<GridController>();
        this._timerController = GameObject.Find("Timer").GetComponent<TimerController>();
        this._leaderboardController = GameObject.Find("Leaderboard").GetComponent<LeaderboardController>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this._gridController.Init();
        this._timerController.Init();
        this._leaderboardController.Init();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the game is finished
        if (!this._hasGameCompleted && this._gridController.IsGameFinished())
        {
            this._hasGameCompleted = true;

            // Record completion time
            float completionTime = this._timerController.Model.GetElapsedTimeFloat();
            this._leaderboardController.History.AddRecord("hehe", completionTime);

            // TODO: Change to on scene feedback
            Debug.Log("YOU WINNNNN");

            GameLog.Instance.WriteToLog("GameManager.cs) The game is finished.");
        }
    }

    void OnApplicationQuit()
    {
        GameLog.Instance.CloseLogger();
    }

    public GridController Grid
    {
        get { return this._gridController; }
    }

    public TimerController Timer
    {
        get { return this._timerController; }
    }

    public bool IsTimerPaused()
    {
        return this._timerController.IsPaused();
    }

    public void RestartGame()
    {
        // Clear game status
        this._gridController.ResetGrid();
        this._timerController.RestartTimer();
        this._hasGameCompleted = false;


        GameLog.Instance.WriteToLog($"(GridController.cs) Game restarted.");
    }
}
