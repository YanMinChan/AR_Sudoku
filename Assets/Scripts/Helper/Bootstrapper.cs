using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

// Initialise controllers and inject database dependency
public class Bootstrapper : MonoBehaviour
{
    [SerializeField]
    private GridController _gridController;

    public void Awake()
    {

    }

    public void Start()
    {
        _gridController.Init(SoundEffectDatabase.Instance, NumberDatabase.Instance, Toaster.Instance);
    }
}
