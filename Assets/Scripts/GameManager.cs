using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Unity changable variables
    [SerializeField] private GridController _gridController;
    [SerializeField] private TimerController _timerController;

    // Instance variables
    private bool _hasGameCompleted = false;
    private LeaderboardHistory _leaderboardhistory;

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
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this._gridController.Init();
        this._timerController.Init();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the game is finished
        if (!this._hasGameCompleted && this._gridController.IsGameFinished())
        {
            this._hasGameCompleted = true;
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
