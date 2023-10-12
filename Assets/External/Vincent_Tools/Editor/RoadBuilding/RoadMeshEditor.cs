using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EditorTool;

[CustomEditor(typeof(RoadMeshCreator))]
public class RoadMeshEditor : Editor {

    RoadMeshCreator m_roadMeshCreator;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (m_roadMeshCreator.roadMaterial == null)
        {
            EditorGUILayout.HelpBox("Assign Road Material", MessageType.Warning);
        }
        EditorGUILayout.Space(15);
        if (GUILayout.Button("Update Mesh") && !m_roadMeshCreator.autoUpdate) {
            m_roadMeshCreator.UpdateRoad();
        }
    }

    void OnSceneGUI()
    {
        if (m_roadMeshCreator.autoUpdate && Event.current.type == EventType.Repaint)
        {
            m_roadMeshCreator.UpdateRoad();
        }
    }

    void OnEnable()
    {
        m_roadMeshCreator = (RoadMeshCreator)target;
        RoadCreator.OnDeletedRoad += m_roadMeshCreator.RemoveRoad;
    }

    private void OnDisable()
    {
        RoadCreator.OnDeletedRoad -= m_roadMeshCreator.RemoveRoad;
    }
}
