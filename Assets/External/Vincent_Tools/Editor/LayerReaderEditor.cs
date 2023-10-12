using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

public class LayerReaderEditor : EditorWindow
{
    private int intFieldWidth = 250;
    private GUIStyle objectNameStyle = new GUIStyle();
    private GUIStyle titleStyle = new GUIStyle();

    private Vector2 scrollPosition = Vector2.zero;

    [MenuItem("VincentTools/LayerReader")]
    public static void ShowWindow()
    {
        GetWindow<LayerReaderEditor>("LayerReader");
    }

    private void OnEnable()
    {
        objectNameStyle.normal.textColor = Color.green;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.normal.textColor = Color.cyan;
    }

    private void OnGUI()
    {
        EditorGUILayout.HelpBox("Select the files that you want to change layer in the scene", MessageType.Info, true);

        if (Selection.activeGameObject != null)
        {
            GameObject currentSelectedParent = Selection.activeGameObject;
            GUILayout.Label("Object To Read Layer", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            //GUILayout.Label(currentSelectedParent.name);

            //Check Selected Object.
            //if (currentSelectedParent.TryGetComponent(out SpriteRenderer parentSR))
            //{
            //    EditorGUI.BeginChangeCheck();
            //    int newId = DrawSortingLayersPopup(parentSR.sortingLayerID);
            //    if (EditorGUI.EndChangeCheck())
            //    {
            //        parentSR.sortingLayerID = newId;
            //    }
            //    parentSR.sortingOrder = EditorGUILayout.IntField("Sorting Order", parentSR.sortingOrder, GUILayout.Width(intFieldWidth));
            //}
            //else if (currentSelectedParent.TryGetComponent(out ParticleSystemRenderer parentPSR))
            //{
            //    EditorGUI.BeginChangeCheck();
            //    int newId = DrawSortingLayersPopup(parentPSR.sortingLayerID);
            //    if (EditorGUI.EndChangeCheck())
            //    {
            //        parentPSR.sortingLayerID = newId;
            //    }
            //    parentPSR.sortingOrder = EditorGUILayout.IntField("Sorting Order", parentPSR.sortingOrder, GUILayout.Width(intFieldWidth));
            //}

            //Check Child Objects.
            Transform currentSelectedParentTransform = currentSelectedParent.transform;
            SpriteRenderer[] childSRs = currentSelectedParent.GetComponentsInChildren<SpriteRenderer>();
            ParticleSystemRenderer[] childPSRs = currentSelectedParent.GetComponentsInChildren<ParticleSystemRenderer>();

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true, GUILayout.Width(350), GUILayout.Height(500));
            GUILayout.Label("Sprite Renderer", titleStyle);
            DrawChildCompoenet(childSRs);
            GUILayout.Label("ParticleSystem", titleStyle);
            DrawChildCompoenet(childPSRs);
            GUILayout.EndScrollView();
        }
        Repaint();
    }

    private void DrawChildCompoenet(Renderer[] _comArray) {
        if (_comArray.Length > 0)
        {
            for (int i = 0; i < _comArray.Length; i++)
            {
                Renderer singleRender = _comArray[i];
                string objectName = singleRender.name;
                GUILayout.Label("Name: " + objectName, objectNameStyle);

                EditorGUI.BeginChangeCheck();
                int newId = DrawSortingLayersPopup(singleRender.sortingLayerID);
                if (EditorGUI.EndChangeCheck())
                {
                    singleRender.sortingLayerID = newId;
                }

                singleRender.sortingOrder = EditorGUILayout.IntField("Sorting Order", singleRender.sortingOrder, GUILayout.Width(intFieldWidth));
                EditorGUILayout.Space();
            }
        }
    }

    private int DrawSortingLayersPopup(int layerID)
    {
        var layers = SortingLayer.layers;
        var names = layers.Select(l => l.name).ToArray();
        if (!SortingLayer.IsValid(layerID))
        {
            layerID = layers[0].id;
        }
        var layerValue = SortingLayer.GetLayerValueFromID(layerID);
        var newLayerValue = EditorGUILayout.Popup("Sorting Layer", layers.ToList().FindIndex(l => l.id == layerID), names);
        return layers[newLayerValue].id;
    }
}
