using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BasicGameAssetManager))]
public class BasicGameAssetEditor : Editor
{
    private BasicGameAssetManager m_basicGameAssetManager;
    private void OnEnable()
    {
        m_basicGameAssetManager = target as BasicGameAssetManager;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Refresh List")) {
            RefreshList();
        }
    }

    void RefreshList() {
        int visualEffectLength = 0;
        int soundEffectLength = 0;
        int UISoundEffectLength = 0;

        visualEffectLength = Enum.GetNames(typeof(VisualEffect)).Length;
        soundEffectLength = Enum.GetNames(typeof(SoundEffect)).Length;
        UISoundEffectLength = Enum.GetNames(typeof(UI_SoundEffects)).Length;

        while (m_basicGameAssetManager.visualEffects.Count < visualEffectLength)
        {
            m_basicGameAssetManager.visualEffects.Add(new V_Effect());
        }
        for (int i = 0; i < m_basicGameAssetManager.visualEffects.Count; i++)
        {
            VisualEffect ve = (VisualEffect)Enum.Parse(typeof(VisualEffect), Enum.GetNames(typeof(VisualEffect))[i]);
            m_basicGameAssetManager.visualEffects[i].effect = ve;
        }

        while (m_basicGameAssetManager.soundEffects.Count < soundEffectLength)
        {
            m_basicGameAssetManager.soundEffects.Add(new S_Effect());
        }
        for (int i = 0; i < m_basicGameAssetManager.soundEffects.Count; i++)
        {
            SoundEffect se = (SoundEffect)Enum.Parse(typeof(SoundEffect), Enum.GetNames(typeof(SoundEffect))[i]);
            m_basicGameAssetManager.soundEffects[i].effect = se;
        }

        while (m_basicGameAssetManager.UI_SoundClipArray.Count < UISoundEffectLength)
        {
            m_basicGameAssetManager.UI_SoundClipArray.Add(new S_UI_Effect());
        }
        for (int i = 0; i < m_basicGameAssetManager.UI_SoundClipArray.Count; i++)
        {
            UI_SoundEffects uise = (UI_SoundEffects)Enum.Parse(typeof(UI_SoundEffects), Enum.GetNames(typeof(UI_SoundEffects))[i]);
            m_basicGameAssetManager.UI_SoundClipArray[i].effect = uise;
        }
    }
}
