using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ToastUI
{
    private float _removalTimer;
    private GameObject _instance;
    private RectTransform _rect;

    public GameObject Instance {  get { return _instance; } }
    public RectTransform Rect { get { return _rect; } set { _rect = value; } }

    public ToastUI(float timer, GameObject instance)
    {
        _removalTimer = timer;
        _instance = instance;
        _rect = _instance.GetComponent<RectTransform>();
    }

    public IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(_removalTimer);
        Toaster.Instance.Remove(this);
    }

}
