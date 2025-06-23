using UnityEngine;

public class TimerNumberController : MonoBehaviour
{
    // Unity accessible variables
    [SerializeField]
    private int pos;
    [SerializeField]
    private GameObject _timerNumber;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject TimerNumber
    {
        get { return _timerNumber; }
        set { _timerNumber = value; }
    }

    public int Position
    {
        get { return pos; }
    }

    public void DisplayDigit()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
