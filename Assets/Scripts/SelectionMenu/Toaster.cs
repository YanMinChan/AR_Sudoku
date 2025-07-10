using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Toaster : MonoBehaviour, IToaster
{
    [SerializeField]
    private GameObject _toastPrefab;

    public static Toaster Instance { get; private set; }

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
            _tUI = new List<ToastUI>();
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

    public void Show(string message, float timer = 2) 
    {
        float oriPosX = _toastPrefab.transform.position.x;

        ToastUI ui = new ToastUI(timer, Instantiate(_toastPrefab, transform));
        StartCoroutine(ui.StartTimer());

        ui.Instance.GetComponentInChildren<TMP_Text>().text = message;

        _tUI.Add(ui);
    }

    public static void Remove(ToastUI toastUI)
    {
        Destroy(toastUI.Instance);
    }
}
