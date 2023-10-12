using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using PolygonTool;
using Sebastian.Geometry;

namespace Vincent.EditorTool
{
    public class AreaDisplayCreator : MonoBehaviour
    {
        public Material meshMaterial;
        public float areaHeight = 0;

        [HideInInspector] public int areaIndex = 0;
        [HideInInspector] public List<Vector3> points = new List<Vector3>();
        private List<Vector3> oriPoints = new List<Vector3>();
        [HideInInspector] public bool canAddNameForArea;
        [HideInInspector] public string nameForArea;
        [HideInInspector] public bool canAddComponentForArea;
        [HideInInspector] public TextAsset script;


        public List<GameObject> areaList = new List<GameObject>();

        #region Public Methods
        public void UpdateMeshDisplay()
        {
            if (points.Count > 2)
            {
                string areaName = canAddNameForArea ? "Area_" + nameForArea : "Area_" + areaIndex;
                GameObject GO_Area = new GameObject(areaName, typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshDataContainer));
                areaList.Add(GO_Area);

                GO_Area.transform.position = GetAreaPivotPoint();
                //Inverse vertix points to pivot point
                for (int i = 0; i < points.Count; i++)
                {
                    oriPoints.Add(points[i]);
                    points[i] = GO_Area.transform.InverseTransformPoint(points[i]);
                }

                areaIndex++;
                GO_Area.transform.SetParent(transform);
                MeshFilter meshFilter = GO_Area.GetComponent<MeshFilter>();
                MeshRenderer meshRender = GO_Area.GetComponent<MeshRenderer>();
                MeshDataContainer serMesh = GO_Area.GetComponent<MeshDataContainer>();


                meshFilter.sharedMesh = Triangulation.GenPolyMesh(points, ArePointsCounterClockwise(points[0], points[1], points[2]));
                meshRender.material = meshMaterial;
                serMesh.SetMeshData(meshFilter.sharedMesh);

                //Reset vertix points to original position in order to avoid visual offset error.
                for (int i = 0; i < points.Count; i++)
                {
                    points[i] = oriPoints[i];
                }
                oriPoints.Clear();

                CreateTextOnMesh(areaName, GO_Area.transform, meshFilter.sharedMesh);
            }
        }

        private bool ArePointsCounterClockwise(Vector3 v0, Vector3 v1, Vector3 v2)
        {
            return SideOfLine(v0, v2, v1) == -1;
        }

        private int SideOfLine(Vector3 a, Vector3 b, Vector3 c)
        {
            return (int)Mathf.Sign((c.x - a.x) * (-b.z + a.z) + (c.z - a.z) * (b.x - a.x));
        }

        public void ResetPoints()
        {
            points.Clear();
        }

        public void ResetAreaIndex()
        {
            areaIndex = 0;
        }

        public void ClearArea()
        {
            RemoveNullAreaInList();
            while (areaList.Count > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
                areaList.RemoveAt(0);
            }
            ResetAreaIndex();
        }

        public void RemoveNullAreaInList()
        {
            areaList.RemoveAll(x => x == null);
        }

        public void RebuildArea()
        {
            if (areaList.Count > 0)
            {
                foreach (var area in areaList)
                {
                    area.GetComponent<MeshDataContainer>().Rebuild();
                }
            }
        }
        #endregion

        private Vector3 GetAreaPivotPoint()
        {
            Vector3 pivotPoint = Vector3.zero;
            float x = 0;
            float z = 0;
            for (int i = 0; i < points.Count; i++)
            {
                x += points[i].x;
                z += points[i].z;
            }
            pivotPoint = new Vector3(x / points.Count, areaHeight, z / points.Count);
            return pivotPoint;
        }

        private void CreateTextOnMesh(string _areaName, Transform _parent, Mesh _areaMesh)
        {
            GameObject GO_AreaText = new GameObject("Text" + _areaName, typeof(TextMeshPro), typeof(ContentSizeFitter));
            GO_AreaText.transform.SetParent(_parent, false);
            GO_AreaText.transform.rotation = Quaternion.Euler(90, 0, 0);
            TextMeshPro areaText = GO_AreaText.GetComponent<TextMeshPro>();
            ContentSizeFitter sizeFitter = GO_AreaText.GetComponent<ContentSizeFitter>();

            sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            areaText.text = canAddNameForArea ? nameForArea : areaIndex.ToString();
            float boundX = _areaMesh.bounds.extents.x;
            float boundZ = _areaMesh.bounds.extents.z;
            areaText.fontSize = boundX + boundZ;
        }
    }
}


