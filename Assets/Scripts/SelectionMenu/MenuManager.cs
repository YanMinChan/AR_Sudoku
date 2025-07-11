using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private void Awake()
    {
        GameLogger.Instance.WriteToLog("Application start");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnApplicationQuit()
    {
        GameLogger.Instance.CloseLogger();
    }
}
