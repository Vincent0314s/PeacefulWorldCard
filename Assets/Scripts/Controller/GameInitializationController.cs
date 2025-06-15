using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script execution order is different from other script.
//Check Script Execution Order in Project setting.
public class GameInitializationController : MonoBehaviour
{
    private List<IInitialization> _initializations;
    private List<IInitiazationEnable> _initiazationEnables;
    private void Awake()
    {
        _initializations = new List<IInitialization>(GetComponentsInChildren<IInitialization>());
        _initiazationEnables = new List<IInitiazationEnable>(GetComponentsInChildren<IInitiazationEnable>());
        foreach (var item in _initializations)
        {
            item.IAwake();
        }
    }

    private void OnEnable()
    {
        foreach (var item in _initiazationEnables)
        {
            item.IEnable();
        }
    }

    private void OnDisable()
    {
        foreach (var item in _initiazationEnables)
        {
            item.IDisable();
        }
    }

    private void Start()
    {
        foreach (var item in _initializations)
        {
            item.IStart();
        }
    }
}
