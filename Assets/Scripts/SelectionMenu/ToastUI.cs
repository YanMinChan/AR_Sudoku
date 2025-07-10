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

    public ToastUI(float timer, GameObject instance)
    {
        _removalTimer = timer;
        _instance = instance;
    }

    public IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(_removalTimer);
        Toaster.Remove(this);
    }

}
