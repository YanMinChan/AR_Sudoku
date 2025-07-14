using MixedReality.Toolkit.UX;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuButtonController : MonoBehaviour
{
    [SerializeField]
    private GameManager _gameManager;
    [SerializeField]
    private TMP_Text _pauseResumeLabel;
    [SerializeField]
    private FontIconSelector _pauseResumeIcon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnUndoButtonPressed()
    {
        SoundEffectDatabase.Instance.PlayAudio(1); // button click sfx
        UIEvents.UndoPressed();
    }

    public void OnPauseButtonPressed()
    {
        // Handle audio feedback
        SoundEffectDatabase.Instance.PlayAudio(1); // button click sfx


        // Handle visual and update logic
        TimerController ctr = this._gameManager.Timer;
        if (!ctr.IsPaused())
        {
            ctr.PauseGame();
            this._pauseResumeLabel.text = "Resume";
            this._pauseResumeIcon.CurrentIconName = "Icon 122";
        }
        else
        {
            ctr.ContinueGame();
            this._pauseResumeLabel.text = "Pause";
            this._pauseResumeIcon.CurrentIconName = "Icon 96";
        }
    }

    public void OnRestartButtonPressed()
    {
        // If the game is previously paused, restart it and reset the button visuals
        if (this._gameManager.Timer.IsPaused())
        {
            this._pauseResumeLabel.text = "Pause";
            this._pauseResumeIcon.CurrentIconName = "Icon 96";
        }

        this._gameManager.RestartGame();
    }

    public void OnMainMenuButtonPressed()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
