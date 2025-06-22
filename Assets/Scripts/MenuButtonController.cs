using UnityEngine;

public class MenuButtonController : MonoBehaviour
{
    private GridController _gridController;

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
}
