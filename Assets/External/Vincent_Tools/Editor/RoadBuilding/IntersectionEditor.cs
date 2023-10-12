using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EditorTool;


[CustomEditor(typeof(Intersection))]
public class IntersectionEditor : Editor
{
    Intersection m_intersection;

    public override void OnInspectorGUI()
    {
        //TODO : Deleted base
        base.OnInspectorGUI();
        EditorGUI.BeginChangeCheck();
        m_intersection.intersectinoSize = EditorGUILayout.Slider("Intersection Size", m_intersection.intersectinoSize, 1, 20);
        if (EditorGUI.EndChangeCheck())
        {
            m_intersection.UpdateSize();
        }
    }

    private void OnEnable()
    {
        m_intersection = target as Intersection;
    }
}
