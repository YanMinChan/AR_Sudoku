using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ToastController : MonoBehaviour
{
    [SerializeField]
    private GameObject _toastPrefab;

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
        Show("Yes", 3);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Show(string message, float timer) 
    {
        ToastUI ui = new ToastUI(timer, Instantiate(_toastPrefab, transform));

        ui.Instance.GetComponentInChildren<TMP_Text>().text = message;

        StartCoroutine(ui.StartTimer());
    }

    //public static void Remove()
    //{
    //    Destroy();
    //}
}
