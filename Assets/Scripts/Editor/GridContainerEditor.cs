using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Core.Card;

[CustomEditor(typeof(GridContainer))]
public class GridContainerEditor : Editor
{

    private GridContainer _gridContainer;
    private void OnEnable()
    {
        _gridContainer = target as GridContainer;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Craete Grid", GUILayout.Width(150), GUILayout.Height(25)))
        {
            _gridContainer.CreateGrid();
        }
    }
}
