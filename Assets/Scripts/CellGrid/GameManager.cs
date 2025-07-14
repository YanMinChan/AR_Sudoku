using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _keyboard;
    [SerializeField]
    private TMP_Text _inputBox;
    [SerializeField]
    private Button _enter;

    // Instance variables
    private GridController _gridController;
    private TimerController _timerController;
    private LeaderboardController _leaderboardController;

    private bool _hasGameCompleted = false;
    private bool _inputReceived = false;

    private ISoundEffectDatabase _sfxDatabase;
    private IToaster _toast;

    private List<IGameObserver> _observers;

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

        _sfxDatabase = SoundEffectDatabase.Instance;
        _toast = Toaster.Instance;

        _observers = new List<IGameObserver>();

        _enter.onClick.AddListener(RecordPlayerInfo);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //_toast.Show("Game started!", 2);
        this._gridController.Init(SoundEffectDatabase.Instance, NumberDatabase.Instance, Toaster.Instance);
        this._timerController.Init();
        this._leaderboardController.Init();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the game is finished
        if (!this._hasGameCompleted && this._gridController.IsGameFinished())
        {
            // Pause timer
            this._hasGameCompleted = true;
            this._timerController.Model.PauseTimer();

            // Audio feedback (Also add onscreen feedback later)
            _sfxDatabase.PlayAudio(6);

            _keyboard.SetActive(true);
            StartCoroutine(WaitForUserInputCoroutine());

            // GameLog.Instance.WriteToLog("GameManager.cs) The game is finished.");
        }
    }

    void OnApplicationQuit()
    {
        GameLogger.Instance.CloseLogger();
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


        GameLogger.Instance.WriteToLog($"(GridController.cs) Game restarted.");
    }

    private IEnumerator WaitForUserInputCoroutine()
    {
        _inputReceived = false;

        yield return new WaitUntil(() => _inputReceived);

        _keyboard.SetActive(false);
    }

    private void RecordPlayerInfo()
    {
        string name = Regex.Replace(_inputBox.text, @"\t|\n|\r", "");
        float completionTime = this._timerController.Model.GetElapsedTimeFloat();
        this._leaderboardController.History.AddRecord(name, completionTime);
        _inputReceived = true;
    }

    public void AddObserver(IGameObserver observer)
    {
        _observers.Add(observer);
    }

    public void NotifyObservers() 
    {
        foreach (var o in _observers) 
        {
            o.Execute(IsTimerPaused());
            Debug.Log(IsTimerPaused());
        }
    }
}
