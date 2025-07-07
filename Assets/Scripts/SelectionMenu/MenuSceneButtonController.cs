using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class MenuSceneButtonController : MonoBehaviour
{
    // Unity changeable variable
    [SerializeField] private GameObject _leaderboard;
    [SerializeField] private TMP_Text _leaderboardButton;
    // Instance variable
    private bool _isLeaderBoardVisible = false;
    private LeaderboardController _lbController;

    // Awake is called on the start of the application
    private void Awake()
    {
        this._lbController = _leaderboard.GetComponent<LeaderboardController>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartGameButtonPressed()
    {
        SceneManager.LoadScene("SudokuScene");
    }

    public void OnLeaderboardButtonClicked()
    {
        // Original position
        Vector3 targetPosition = new Vector3(0, 1.6f, 0.2f);
        Quaternion targetRotation = Quaternion.identity;

        if (_isLeaderBoardVisible) 
        {
            // Disable leaderboard
            _isLeaderBoardVisible = false;
            _leaderboard.SetActive(false);
            _leaderboardButton.text = "Leaderboard";
        }
        else
        {
            // Move menu to side
            targetPosition = new Vector3(-0.1f, 1.6f, 0.2f);
            targetRotation = Quaternion.Euler(0f, -8f, 0f);

            // Show leaderboard
            _isLeaderBoardVisible = true;
            _leaderboard.SetActive(true);
            this._lbController.GetLeaderboardTMP();
            _leaderboardButton.text = "Close Leaderboard";
        }

        // Transition the location of the menu
        StartCoroutine(MoveAndRotateMenu(transform, targetPosition, targetRotation, 0.2f));
    }

    public void OnQuitButtonPressed()
    {
        GameLogger.Instance.WriteToLog("Application quit");
        // Quit editor if it's in editor version
        if (EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
        }
        // Rmb to also handle application quit event like logs
        else
        {
            Application.Quit();
        }
    }

    private IEnumerator MoveAndRotateMenu(Transform menuTransform, Vector3 targetPos, Quaternion targetRot, float duration)
    {
        Vector3 startPos = menuTransform.position;
        Quaternion startRot = menuTransform.rotation;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            menuTransform.position = Vector3.Lerp(startPos, targetPos, t);
            menuTransform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure final position/rotation
        menuTransform.position = targetPos;
        menuTransform.rotation = targetRot;

    }
}
