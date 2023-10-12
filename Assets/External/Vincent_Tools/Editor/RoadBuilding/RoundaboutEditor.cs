using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Roundabout))]
public class RoundaboutEditor : Editor
{
    Roundabout m_roundAbout;

    [MenuItem("GameObject/Create Other/Procedural Roundabout")]
    public static void Create()
    {
        GameObject GO = new GameObject("P_Roundabout", typeof(Roundabout));
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        m_roundAbout.smoothness = EditorGUILayout.IntSlider("Circle Smoothness", m_roundAbout.smoothness, 1, 20);
        m_roundAbout.radius = EditorGUILayout.Slider("Circle Radius", m_roundAbout.radius, 1, 20);
        m_roundAbout.roadWidth = EditorGUILayout.Slider("Road Width", m_roundAbout.roadWidth, 0.1f, 20);
        if (EditorGUI.EndChangeCheck())
        {
            m_roundAbout.UpdateRoundabout();
        }
    }

    private void OnEnable()
    {
        m_roundAbout = target as Roundabout;
        m_roundAbout.UpdateRoundabout();
    }
}
