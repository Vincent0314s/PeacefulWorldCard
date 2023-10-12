using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EditorTool;

[CustomEditor(typeof(BuildingInitCreator))]
public class BuildingInitEditor : Editor
{
    BuildingInitCreator m_buildingInitCreator;


    [MenuItem("GameObject/Create Other/Procedural Building")]
    public static void Create() {
        GameObject GO = new GameObject("P_Building",typeof(BuildingInitCreator),typeof(BoxCollider));
    }
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        m_buildingInitCreator.buildingFloorNum = EditorGUILayout.IntSlider("buildingFloorNum", m_buildingInitCreator.buildingFloorNum,1,m_buildingInitCreator.maxHeight);
        m_buildingInitCreator.buildingSize = EditorGUILayout.IntSlider("buildingSize", m_buildingInitCreator.buildingSize, 1, 10);
        if (EditorGUI.EndChangeCheck())
        {
            m_buildingInitCreator.GenerateBuilding();
        }

        if(m_buildingInitCreator.baseFloor == null)
            m_buildingInitCreator.baseFloor = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Building/BaseFloor.prefab", typeof(GameObject));
        if(m_buildingInitCreator.baseRoof == null)
            m_buildingInitCreator.baseRoof = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Building/BaseRoof.prefab", typeof(GameObject));
    }

    private void OnEnable()
    {
        m_buildingInitCreator = target as BuildingInitCreator;
    }
}
