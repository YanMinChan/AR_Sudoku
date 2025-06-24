using UnityEngine;
using TMPro;
using MixedReality.Toolkit.UX;
public class MenuButtonController : MonoBehaviour
{
    [SerializeField]
    private GridController _gridController;
    [SerializeField]
    private TimerController _timerController;

    [SerializeField]
    private TMP_Text _pauseResumeLabel;
    //[SerializeField]
    //private TMP_Text _resumeLabel;
    [SerializeField]
    private FontIconSelector _pauseResumeIcon;
    //[SerializeField]
    //private FontIconSelector _resumeIcon;
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
        this._gridController.UndoLastAction();
    }

    public void OnPauseButtonPressed()
    {
        // Handle audio feedback
        SoundEffectDatabase.Instance.PlayAudio(1); // button click sfx
        

        // Handle visual and update logic
        TimerController ctr = this._timerController;
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
}
