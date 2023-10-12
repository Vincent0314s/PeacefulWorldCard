using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

[CustomEditor(typeof(SettingManager))]
public class SettingManagerEditor : Editor
{
    private SettingManager m_SettingManager;

    GUIStyle boldFont;

    private void OnEnable()
    {
        m_SettingManager = target as SettingManager;
        boldFont = new GUIStyle();

        boldFont.fontSize = 14;
        boldFont.fontStyle = FontStyle.Bold;
        boldFont.normal.textColor = Color.white;
    }

    public override void OnInspectorGUI()
    {
        m_SettingManager.textType = (TextType)EditorGUILayout.EnumPopup("Text Type" , m_SettingManager.textType);
        EditorGUILayout.Space();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Resolution", boldFont);
        EditorGUI.indentLevel++;
        if (m_SettingManager.textType == TextType.Normal)
        {
            m_SettingManager.normal_resolutionDisplay = EditorGUILayout.ObjectField("Resolution DropDown", m_SettingManager.normal_resolutionDisplay, typeof(Dropdown), true) as Dropdown;
        }
        else {
            m_SettingManager.tmp_resolutionDisplay = EditorGUILayout.ObjectField("Resolution DropDown", m_SettingManager.tmp_resolutionDisplay, typeof(TMP_Dropdown), true) as TMP_Dropdown;
        }
        EditorGUI.indentLevel--;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Sound",boldFont);
        EditorGUI.indentLevel++;
        m_SettingManager.masterSlider = EditorGUILayout.ObjectField("Master Slider", m_SettingManager.masterSlider, typeof(Slider), true) as Slider;
        m_SettingManager.musicSlider = EditorGUILayout.ObjectField("Music Slider", m_SettingManager.musicSlider, typeof(Slider), true) as Slider;
        m_SettingManager.soundEffectSlider = EditorGUILayout.ObjectField("SFX Slider", m_SettingManager.soundEffectSlider, typeof(Slider), true) as Slider;
        EditorGUI.indentLevel--;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Audio", boldFont);
        EditorGUI.indentLevel++;
        m_SettingManager.masterMixer = EditorGUILayout.ObjectField("Master Mixer", m_SettingManager.masterMixer, typeof(AudioMixer), true) as AudioMixer;
        m_SettingManager.musicOutPutGroup = EditorGUILayout.ObjectField("Master Mixer", m_SettingManager.musicOutPutGroup, typeof(AudioMixerGroup), true) as AudioMixerGroup;
        m_SettingManager.soundEffectOutPutGroup = EditorGUILayout.ObjectField("Master Mixer", m_SettingManager.soundEffectOutPutGroup, typeof(AudioMixerGroup), true) as AudioMixerGroup;
        EditorGUI.indentLevel--;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("FullScreen Toggle", boldFont);
        EditorGUI.indentLevel++;
        m_SettingManager.fullScreenToggle = EditorGUILayout.ObjectField("FullScreen Toggle", m_SettingManager.fullScreenToggle, typeof(Toggle), true) as Toggle;
        EditorGUI.indentLevel--;
    }
}
