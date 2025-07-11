using UnityEngine;
using UnityEngine.Experimental.XR.Interaction;

public class NumberController : MonoBehaviour
{
    [SerializeField][Range(1, 9)]
    private int number;
    private GridController _gridController;

    public int Number 
    {  
        get { return number; } 
        set { number = value; }
    }

    public void Initialize(GridController grid)
    {
        this._gridController = grid;
    }

    // Handles event on number pressed and print debug log
    public void OnNumberPressed()
    {
        this._gridController.FillNumber(this.number);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
