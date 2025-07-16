using MixedReality.Toolkit.UX;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MixedReality.Toolkit.SpatialManipulation;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _keyboard;
    [SerializeField]
    private TMP_Text _inputBox;
    [SerializeField]
    private Button _enter;
    [SerializeField]
    private DialogPool _dialogPool;

    private TimerController _timerController;
    private LeaderboardController _leaderboardController;

    private bool _hasGameCompleted = false;
    private bool _hasPuzzleFinished = false;
    private bool _inputReceived = false;

    private ISoundEffectDatabase _sfxDatabase;
    private IToaster _toast;

    private List<ITimerObserver> _timerObservers;

    public static GameManager Instance { get; private set; }
    public bool HasPuzzleFinished { set {  _hasPuzzleFinished = value; } }

    public List<ITimerObserver> Test { get { return _timerObservers; } }

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
        _timerObservers = new List<ITimerObserver>();

        this._timerController = GameObject.Find("Timer").GetComponent<TimerController>();
        this._leaderboardController = GameObject.Find("Leaderboard").GetComponent<LeaderboardController>();

        _sfxDatabase = SoundEffectDatabase.Instance;
        _toast = Toaster.Instance;

        _enter.onClick.AddListener(RecordPlayerInfo);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //_toast.Show("Game started!", 2);
        this._timerController.Init();
        this._leaderboardController.Init();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the game is finished
        if (!_hasGameCompleted && _hasPuzzleFinished)
        {
            _hasGameCompleted = true;
            StartCoroutine(HandleGameCompleteCoroutine());

            // GameLog.Instance.WriteToLog("GameManager.cs) The game is finished.");
        }
    }

    void OnApplicationQuit()
    {
        GameLogger.Instance.CloseLogger();
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
        _hasGameCompleted = false;
        GameEvents.ResetGame(false);

        GameLogger.Instance.WriteToLog($"(GridController.cs) Game restarted.");
    }

    private IEnumerator HandleGameCompleteCoroutine()
    {
        GameEvents.GameComplete();
        _sfxDatabase.PlayAudio(6);
        
        _keyboard.SetActive(true);
        _inputReceived = false;
        yield return new WaitUntil(() => _inputReceived);
        _keyboard.SetActive(false);

        Dialog d = (Dialog) _dialogPool.Get()
            .SetBody("Start a new game?")
            .SetPositive("Yes", (args) => { GameEvents.NewPuzzle(true); })
            .SetNegative("No", (args) => { SceneManager.LoadScene("MenuScene"); })
            .Show();

        d.gameObject.GetComponent<Follow>().MaxDistance = 0.3f;

        _hasGameCompleted = false;
        _hasPuzzleFinished = false;
    }

    private void RecordPlayerInfo()
    {
        string name = Regex.Replace(_inputBox.text, @"\t|\n|\r", "");
        float completionTime = this._timerController.Model.GetElapsedTimeFloat();
        this._leaderboardController.History.AddRecord(name, completionTime);
        _inputReceived = true;
    }

    public void AddTimerObserver(ITimerObserver observer)
    {
        _timerObservers.Add(observer);
    }

    public void NotifyObservers() 
    {
        foreach (var o in _timerObservers) 
        {
            o.Invoke(IsTimerPaused());
        }
    }
}
