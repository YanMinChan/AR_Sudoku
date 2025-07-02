using UnityEngine;

public class TimerContainerController : MonoBehaviour
{
    // Unity accessible variables
    [SerializeField]
    private int _pos;
    [SerializeField]
    private GameObject _timerContainer;

    // Dependency Injection
    private INumberDatabase _timerDatabase;

    public void Init(INumberDatabase timerDatabase)
    {
        _timerDatabase = timerDatabase;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject TimerContainer
    {
        get { return _timerContainer; }
        set { _timerContainer = value; }
    }

    public int Position
    {
        get { return _pos; }
    }

    public void DisplayDigit(int digits)
    {
        // If there is a digit in the container, destroy it
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        GameObject timerPrefab = _timerDatabase.GetNumber(digits);
        if (timerPrefab != null)
        {
            this._timerContainer = Instantiate(timerPrefab, transform);
            this._timerContainer.transform.localRotation = Quaternion.Euler(0, 180, 0);
            this._timerContainer.transform.localScale = Vector3.one;
        }
    }
}
