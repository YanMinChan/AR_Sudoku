using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ToastController : MonoBehaviour
{
    public GameObject _toastPrefab;

    public static ToastController Instance { get; private set; }

    private List<ToastUI> _tUI;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        } 
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show(string message, float timer) 
    {
        ToastUI ui = new ToastUI(timer, Instantiate(_toastPrefab));

        ui.Instance.GetComponent<TMP_Text>().text = message;
    }
}
