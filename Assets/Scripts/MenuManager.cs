using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private LeaderboardHistory _leaderboardhistory;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnApplicationQuit()
    {
        // Just testing
        this._leaderboardhistory = new LeaderboardHistory();
        this._leaderboardhistory.SaveLeaderboard();
    }
}
