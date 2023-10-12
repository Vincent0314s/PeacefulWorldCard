using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EditorTool;

[CustomEditor(typeof(BuildingCreator))]
public class BuildingEditor : Editor
{
    BuildingCreator m_buildingCreator;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space(25);
        m_buildingCreator.IsBuildingSizeBasedOnRoadSpacing = EditorGUILayout.ToggleLeft("Is Building Size Based On Road Spacing", m_buildingCreator.IsBuildingSizeBasedOnRoadSpacing);
        if (!m_buildingCreator.IsBuildingSizeBasedOnRoadSpacing) {
            m_buildingCreator.buildingSize = EditorGUILayout.Slider("Custom Building Size",m_buildingCreator.buildingSize,1,15f);
        }
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Update Building"))
        {
            m_buildingCreator.UpdateBuilding();
        }

        if (GUILayout.Button("Delete All Buildings"))
        {
            m_buildingCreator.ClearBuildings();
        }
        EditorGUILayout.EndHorizontal();
    }

    void OnEnable()
    {
        m_buildingCreator = (BuildingCreator)target;
        RoadCreator.OnDeletedRoad += m_buildingCreator.RemoveRoad;
    }

    private void OnDisable()
    {
        RoadCreator.OnDeletedRoad -= m_buildingCreator.RemoveRoad;
    }
}
