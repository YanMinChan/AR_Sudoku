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
    private TutorialController _tutController;

    private IMenuPanel _currPanel;

    // Awake is called on the start of the application
    private void Awake()
    {
        this._lbController = _leaderboard.GetComponent<LeaderboardController>();
        this._tutController = _tutorialDialog.GetComponent<TutorialController>();
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
            ref _isTutorialVisible,
            _tutController
        );
    }

    public void OnLeaderboardButtonClicked()
    {
        this.RevealWindow(
            _leaderboard,
            ref _isLeaderBoardVisible,
            _lbController,
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

    private void RevealWindow(GameObject window, ref bool isVisible, IMenuPanel panel, TMP_Text button = null, string[] buttonTexts = null, Action onShow = null)
    {
        // Move the menu panel to side
        if (_currPanel == null) { MenuAtMiddle(false); }

        if (_currPanel == panel)
        {
            // Close side panel
            _currPanel.Close();
            _currPanel = null;

            // Move menu back to middle
            MenuAtMiddle(true);
        }
        else
        {
            // Close earlier panel
            if (_currPanel != null) _currPanel.Close();

            // Show another side panel
            _currPanel = panel;
            window.SetActive(true);
            
            if (button != null && buttonTexts != null) button.text = buttonTexts[1];

            onShow?.Invoke();
        }
    }

    private void MenuAtMiddle(bool def)
    {
        // Original position
        Vector3 targetPos = new Vector3(0, 1.6f, 0.2f);
        Quaternion targetRot = Quaternion.identity;

        // Moving to side
        if (!def)
        {
            // Move menu to side
            targetPos = new Vector3(-0.1f, 1.6f, 0.2f);
            targetRot = Quaternion.Euler(0f, -8f, 0f);
        }

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
