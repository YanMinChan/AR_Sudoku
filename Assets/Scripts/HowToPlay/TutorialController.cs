using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialController : MonoBehaviour, IMenuPanel
{
    private GameObject _tutPane;
    // Start is called before the first frame update
    void Start()
    {
        _tutPane = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Close()
    {
        _tutPane.SetActive(false);
    }
}
