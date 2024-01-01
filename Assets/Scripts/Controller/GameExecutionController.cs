using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script execution order is different from other script.
//Check Script Execution Order in Project setting.
public class GameExecutionController : MonoBehaviour
{
    private List<IInitialization> initializations;
    private void Awake()
    {
        initializations = new List<IInitialization>(GetComponentsInChildren<IInitialization>());
        for (int i = 0; i < initializations.Count; i++)
        {
            initializations[i].IAwake();
        }
    }

    private void Start()
    {
        for (int i = 0; i < initializations.Count; i++)
        {
            initializations[i].IStart();
        }
    }
}
