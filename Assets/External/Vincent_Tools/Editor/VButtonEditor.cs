using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(VButton))]
public class VButtonEditor : Editor
{
    private bool showSFX;
    private bool showButtonEvents;
    Image image;

    //private string normal = "Normal";
    //private string highlighted = "HighLighted";
    //private string clicked = "Clicked";


    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        //SetUp Target to get access
        VButton vButton = target as VButton;
        image = vButton.GetComponent<Image>();

        //Show Transition preperty
        SerializedProperty transitionType = serializedObject.FindProperty("Transition");
        EditorGUILayout.PropertyField(transitionType);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Main Sprite");
        vButton.mainSprite = (Sprite)EditorGUILayout.ObjectField(vButton.mainSprite, typeof(Sprite), allowSceneObjects: false);
        EditorGUILayout.EndHorizontal();
        if (image != null && !EditorApplication.isPlaying) {
            image.sprite = vButton.mainSprite;
        }

        //Determine what kind of transtion that Button will show.
        if (vButton.Transition == VButton.transition.Color)
        {
            if (image == null)
            {
                EditorGUILayout.HelpBox("Must have Image component",MessageType.Error);
            }

            EditorGUI.indentLevel++;
            vButton.normalColor = EditorGUILayout.ColorField("Normal Color", vButton.normalColor);
            vButton.highLightedColor = EditorGUILayout.ColorField("Highlighted Color", vButton.highLightedColor);
            vButton.clickedColor = EditorGUILayout.ColorField("Clicked Color", vButton.clickedColor);
            EditorGUI.indentLevel--;

            if (!Application.isPlaying) { 
                image.color = vButton.normalColor;
            }
            //if (Application.isPlaying)
            //{
            //    //Change Color
            //    if (vButton.mouseState == V_Button.MouseState.Hover)
            //    {
            //        vButton.OnEnter.AddListener(() =>
            //        {
            //            Debug.Log("Highlight");
            //            image.color = vButton.highLightedColor;
            //        });
            //    }
            //    else if (vButton.mouseState == V_Button.MouseState.Click)
            //    {
            //        vButton.OnClick.AddListener(() =>
            //        {
            //            image.color = vButton.clickedColor;
            //        });
            //    }
            //    else
            //    {
            //        image.color = vButton.normalColor;
            //    }
            //}
            //else {
            //    image.color = vButton.normalColor;
            //}
        } else if (vButton.Transition == VButton.transition.Sprite) {
            image.color = Color.white;
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Normal Sprite");
            vButton.normalSprite = (Sprite)EditorGUILayout.ObjectField(vButton.normalSprite,typeof(Sprite),allowSceneObjects :false);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Highlighted Sprite");
            vButton.highLightedSprite = (Sprite)EditorGUILayout.ObjectField(vButton.highLightedSprite, typeof(Sprite), allowSceneObjects: false);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Clicked Sprite");
            vButton.clickedSprite = (Sprite)EditorGUILayout.ObjectField(vButton.clickedSprite, typeof(Sprite), allowSceneObjects: false);
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel--;

            if (!Application.isPlaying) { 
                image.sprite = vButton.normalSprite;
            }
            //if (EditorApplication.isPlaying) {
            //    //Change sprtie
            //    if (vButton.mouseState == V_Button.MouseState.Hover)
            //    {
            //        vButton.OnEnter.AddListener(() =>
            //        {
            //            image.sprite = vButton.highLightedSprite;
            //        });
            //    }
            //    else if (vButton.mouseState == V_Button.MouseState.Click)
            //    {
            //        if (vButton.clickedSprite != null)
            //        {
            //            vButton.OnClick.AddListener(() =>
            //            {
            //                image.sprite = vButton.clickedSprite;
            //            });
            //        }
            //        else {
            //            image.sprite = vButton.normalSprite;
            //        }
            //    }
            //    else
            //    {
            //        image.sprite = vButton.normalSprite;
            //    }
            //}
        }

        vButton.ableToChangeScene = GUILayout.Toggle(vButton.ableToChangeScene, "Able to change scene");
        if (vButton.ableToChangeScene)
        {
            EditorGUI.indentLevel++;
            vButton.sceneToChange = (SceneInTheBuild)EditorGUILayout.EnumPopup("Scene to change", vButton.sceneToChange);
            EditorGUI.indentLevel--;
        }


        showSFX = GUILayout.Toggle(showSFX, "Show SFX");
        if (showSFX) {
            EditorGUI.indentLevel++;
            vButton.ableToAssignSFX = EditorGUILayout.Toggle("Able to play SFX", vButton.ableToAssignSFX);
            vButton.hover = (UI_SoundEffects)EditorGUILayout.EnumPopup("Hover SFX", vButton.hover);
            vButton.click = (UI_SoundEffects)EditorGUILayout.EnumPopup("Click SFX", vButton.click);
            vButton.exit = (UI_SoundEffects)EditorGUILayout.EnumPopup("Exit SFX", vButton.exit);
            EditorGUI.indentLevel--;
        }

        showButtonEvents = GUILayout.Toggle(showButtonEvents,"ShowEvents");
        if (showButtonEvents)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("OnEnter"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("OnClick"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("OnExit"));
        }
      
        serializedObject.ApplyModifiedProperties();

        EditorUtility.SetDirty(vButton);

    }
}
