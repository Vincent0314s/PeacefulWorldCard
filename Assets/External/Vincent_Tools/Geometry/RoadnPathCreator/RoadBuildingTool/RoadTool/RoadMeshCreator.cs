using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EditorTool {
    [RequireComponent(typeof(BuildingCreator))]
    [RequireComponent(typeof(RoadCreator))]
    public class RoadMeshCreator : MonoBehaviour
    {
        [Range(.05f, 25f)]
        public float spacing = 1;
        public float roadWidth = 1;
        public bool autoUpdate;
        public Material roadMaterial;

        private RoadCreator m_roadCreator;
        [SerializeField]
        private List<GameObject> roadMeshObjects = new List<GameObject>();
        public GameObject currentRoadMeshObject => roadMeshObjects[m_roadCreator.currentRoadIndex];

        public Vector3[] verts { get; private set; }
        public Vector3[] segments { get; private set; }

        private Road currentPath;

        private void OnValidate()
        {
            if (m_roadCreator == null)
                m_roadCreator = GetComponent<RoadCreator>();
        }

        public void UpdateRoad()
        {
            if (roadMeshObjects.Count == 0)
            {
                GameObject newRoadGO = new GameObject("Road", typeof(MeshFilter), typeof(MeshRenderer));
                newRoadGO.transform.SetParent(this.transform);
                if (!roadMeshObjects.Contains(newRoadGO))
                    roadMeshObjects.Add(newRoadGO);
            }
            else if (roadMeshObjects.Count - 1 < m_roadCreator.currentRoadIndex)
            {
                GameObject newRoadGO = new GameObject("Road", typeof(MeshFilter), typeof(MeshRenderer));
                newRoadGO.transform.SetParent(this.transform);
                if (!roadMeshObjects.Contains(newRoadGO))
                    roadMeshObjects.Add(newRoadGO);
            }

            currentPath = m_roadCreator.Path;
            Vector3[] points = currentPath.CalculateEvenlySpacedPoints(spacing);
            segments = points;
            MeshFilter mf = roadMeshObjects[m_roadCreator.currentRoadIndex].GetComponent<MeshFilter>();
            MeshRenderer mr = roadMeshObjects[m_roadCreator.currentRoadIndex].GetComponent<MeshRenderer>();
            mf.sharedMesh = ExtensionMethods.CreateRoadMesh(points, roadWidth, currentPath.IsClosed);
            verts = mf.sharedMesh.vertices;
            mr.material = roadMaterial;
        }

        public void RemoveRoad(int _index) {
            if (roadMeshObjects.Count > _index) {
                DestroyImmediate(roadMeshObjects[_index]);
                roadMeshObjects.RemoveAt(_index);
            }
        }
    }
}
