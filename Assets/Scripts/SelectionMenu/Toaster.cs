using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System.Linq;

public class Toaster : MonoBehaviour, IToaster
{
    [SerializeField]
    private GameObject _toastPrefab;

    public static Toaster Instance { get; private set; }

    private List<ToastUI> _toastList;
    private float _startingPosAnchorY = 100, _spacing = 10;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        } 
        else
        {
            Instance = this;
            _toastList = new List<ToastUI>();
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

    public ToastUI Show(string message, float timer = 15) 
    {
        float oriPosX = _toastPrefab.transform.position.x;

        ToastUI ui = new ToastUI(timer, Instantiate(_toastPrefab, transform));
        //ui.Rect = ui.Instance.GetComponent<RectTransform>();

        //ui.Instance.transform.position = new Vector3();
        ui.Instance.GetComponentInChildren<TMP_Text>().text = message;

        _toastList.Add(ui);
        StartCoroutine(ui.StartTimer());

        Sync();

        return ui;
    }

    public void Remove(ToastUI toastUI)
    {
        if (toastUI == null) return;

        Destroy(toastUI.Instance);
        _toastList.Remove(toastUI);

        Sync();
    }

    public void Sync()
    {
        ToastUI ui = _toastList.FirstOrDefault();
        float i = 0;
        if (ui == null) return;

        foreach(ToastUI toast in _toastList)
        {
            float anchorPosY = _startingPosAnchorY - (toast.Rect.sizeDelta.y * i) + _spacing;
            Vector2 tmpPos = toast.Rect.anchoredPosition;
            tmpPos.y = anchorPosY;

            toast.Rect.anchoredPosition = tmpPos;
            i++;
        }

    }
}
