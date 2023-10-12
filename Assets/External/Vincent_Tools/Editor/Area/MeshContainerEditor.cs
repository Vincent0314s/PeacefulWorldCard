using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshDataContainer))]
public class MeshContainerEditor : Editor
{
    MeshDataContainer m_meshContainer;

    private void OnEnable()
    {
        m_meshContainer = target as MeshDataContainer;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Rebuild"))
        {
            if (m_meshContainer)
            {
                m_meshContainer.Rebuild();
            }
        }
    }
}
