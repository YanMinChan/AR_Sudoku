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
    [SerializeField] private GameObject _leaderboardGameObject;
    [SerializeField] private GameObject _tutorialGameObject;

    // Instance variable
    private LeaderboardController _lbCtr;
    private TutorialController _tutCtr;

    private IMenuPanel _currPanel;

    public class Panel {
        public GameObject go;
        public IMenuPanel panelCtr;
        public Action onShow = null;
    }

    // Awake is called on the start of the application
    private void Awake()
    {
        _lbCtr = _leaderboardGameObject.GetComponent<LeaderboardController>();
        _tutCtr = _tutorialGameObject.GetComponent<TutorialController>();
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
        Panel tutorial = new Panel { 
            go = _tutorialGameObject, 
            panelCtr = _tutCtr 
        };

        // Show the tutorial dialog
        this.OpenPanel(tutorial);
    }

    public void OnLeaderboardButtonClicked()
    {
        Panel leaderboard = new Panel { 
            go = _leaderboardGameObject, 
            panelCtr = _lbCtr, 
            onShow = () => _lbCtr.GetLeaderboardTMP()
        };

        this.OpenPanel(leaderboard);
    }

    public void OnQuitButtonClicked()
    {
        GameLogger.Instance.WriteToLog("Application quit");
        // Quit editor if it's in editor version (comment this out on build)
        //if (EditorApplication.isPlaying)
        //{
        //    EditorApplication.isPlaying = false;
        //}
        // Rmb to also handle application quit event like logs
        Application.Quit();
    }

    private void OpenPanel(Panel panel)
    {
        // Move the menu panel to side
        if (_currPanel == null) { MenuAtMiddle(false); }

        if (_currPanel == panel.panelCtr)
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
            _currPanel = panel.panelCtr;
            panel.go.SetActive(true);

            panel.onShow?.Invoke();
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
