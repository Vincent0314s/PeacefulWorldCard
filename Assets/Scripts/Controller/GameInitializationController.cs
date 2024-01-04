using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script execution order is different from other script.
//Check Script Execution Order in Project setting.
public class GameInitializationController : MonoBehaviour
{
    private List<IInitialization> _initializations;
    private void Awake()
    {
        _initializations = new List<IInitialization>(GetComponentsInChildren<IInitialization>());
        for (int i = 0; i < _initializations.Count; i++)
        {
            _initializations[i].IAwake();
        }
    }

    private void Start()
    {
        for (int i = 0; i < _initializations.Count; i++)
        {
            _initializations[i].IStart();
        }
    }
}
