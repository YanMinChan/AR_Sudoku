using UnityEngine;
using TMPro;
public class MenuButtonController : MonoBehaviour
{
    [SerializeField]
    private GridController _gridController;
    [SerializeField]
    private TimerController _timerController;

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
        this._gridController.UndoLastAction();
    }

    public void OnPauseButtonPressed()
    {
        TimerController ctr = this._timerController;
        if (!ctr.IsPaused())
        {
            // GameObject button = ctr.gameObject.transform.Find("Pause").GetComponent<>;
            ctr.PauseGame();
        }
        else
        {
            ctr.ContinueGame();
        }
    }
}
