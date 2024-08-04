using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpriteListSO), true)]
public class SpriteListSOEditor : Editor
{
    private SpriteListSO _spriteSO;

    private void OnEnable()
    {
        if (_spriteSO == null)
        {
            _spriteSO = target as SpriteListSO;
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();

        if (GUILayout.Button("Load All Sprites From Path"))
        {
            string fullPath = $"{Application.dataPath}/{_spriteSO.folderPath}";
            if (!System.IO.Directory.Exists(fullPath))
            {
                Debug.LogError("Folder doesnt Exist!");
                return;
            }

            var folders = new string[] { $"Assets/{_spriteSO.folderPath}" };
            var guids = AssetDatabase.FindAssets("t:Sprite", folders);

            var newSprites = new Sprite[guids.Length];
            for (int i = 0; i < newSprites.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                newSprites[i] = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            }
            _spriteSO.Sprites = newSprites;
        }
    }
}
