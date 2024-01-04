using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicController : MonoBehaviour
{
    private List<IUpdateLoop> _iUpdateLoops;

    private void Awake()
    {
        _iUpdateLoops = new List<IUpdateLoop>(GetComponentsInChildren<IUpdateLoop>());
    }

    private void Update()
    {
        for (int i = 0; i < _iUpdateLoops.Count; i++)
        {
            _iUpdateLoops[i].IUpdate();
        }
    }
}
