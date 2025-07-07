using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using System;
using MixedReality.Toolkit.UX;

public class MenuSceneButtonController : MonoBehaviour
{
    // Unity changeable variable
    [SerializeField] private GameObject _leaderboard;
    [SerializeField] private GameObject _tutorialDialog;
    [SerializeField] private TMP_Text _leaderboardButton;

    // Instance variable
    private bool _isLeaderBoardVisible = false;
    private bool _isTutorialVisible = false;

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

    public void OnStartGameButtonClicked()
    {
        SceneManager.LoadScene("SudokuScene");
    }

    public void OnTutorialButtonClicked() 
    {
        // Show the tutorial dialog
        this.RevealWindow(
            _tutorialDialog,
            ref _isTutorialVisible
        );
    }

    public void OnLeaderboardButtonClicked()
    {
        this.RevealWindow(
            _leaderboard,
            ref _isLeaderBoardVisible,
            _leaderboardButton,
            new[] { "Leaderboard", "Close Leaderboard"},
            () => _lbController.GetLeaderboardTMP()
        );
    }

    public void OnQuitButtonClicked()
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

    private void RevealWindow(GameObject window, ref bool isVisible, TMP_Text button = null, string[] buttonTexts = null, Action onShow = null)
    {
        // Original position
        Vector3 targetPos = new Vector3(0, 1.6f, 0.2f);
        Quaternion targetRot = Quaternion.identity;

        if (isVisible)
        {
            // Disable leaderboard
            isVisible = false;
            window.SetActive(false);
            if (button != null && buttonTexts != null) button.text = buttonTexts[0];
        }
        else
        {
            // Move menu to side
            targetPos = new Vector3(-0.1f, 1.6f, 0.2f);
            targetRot = Quaternion.Euler(0f, -8f, 0f);

            // Show leaderboard
            isVisible = true;
            window.SetActive(true);
            if (button != null && buttonTexts != null) button.text = buttonTexts[1];

            onShow?.Invoke();
        }

        // Transition the location of the menu
        StartCoroutine(MoveAndRotateMenu(this.transform, targetPos, targetRot, 0.2f));
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
