using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class LeaderboardController: MonoBehaviour, IMenuPanel
{
    private GameObject _lbPane;
    private LeaderboardHistory _history;
    private List<TMP_Text> _displays;

    void Awake()
    {
        Init();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEnable()
    {
        GameEvents.OnAddPlayerRecord += _history.AddRecord;
    }

    public void OnDestroy()
    {
        this._history.SaveLeaderboard();
    }

    public LeaderboardHistory History
    {
        get { return _history; }
        set { _history = value; }
    }

    public void Init()
    {
        _lbPane = this.gameObject;

        _history = new LeaderboardHistory();
        _history.LoadLeaderboard();

        _displays = GameObject.FindGameObjectsWithTag("LbRecord")
            .SelectMany(obj => obj.GetComponentsInChildren<TMP_Text>(true))
            .ToList();
    }

    /// <summary>
    /// Generate the TMP text on scene
    /// </summary>
    public void GetLeaderboardTMP()
    {
        List<string> records = this._history.GenerateEntriesString().EntriesText;

        for(int i = 0; i < _displays.Count; i++)
        {
            if (i < records.Count)
            {
                _displays[i].text = records[i];
            }
            else
            {
                _displays[i].text = "No record yet";
            }
        }
    }

    public void Close()
    {
        _lbPane.SetActive(false);
    }
}
