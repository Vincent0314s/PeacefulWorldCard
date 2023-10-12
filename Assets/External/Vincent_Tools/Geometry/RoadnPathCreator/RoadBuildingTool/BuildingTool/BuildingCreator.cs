using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace EditorTool {
    public class BuildingCreator : MonoBehaviour
    {
        public BuildingInitCreator PF_proceduralBuilding;
        [Range(1, 3)]
        public int buildingLayer = 1;
        public float buildingLayerSpacing = 2.4f;
        public float buildinfOffsetFromRoad = 1.2f;
        public float angleToRemoveBuilding = 15f;

        [HideInInspector]public bool IsBuildingSizeBasedOnRoadSpacing = false;
        [HideInInspector]public float buildingSize = 1f;
        [Min(1)]
        public int buildingSpacing = 2;
        [Min(0)]
        public int buildingRemoveHeadNum = 1;
        

        private RoadMeshCreator m_roadMeshCreator;
        private RoadCreator m_roadCreator;
        public List<GameObject> buildingParents = new List<GameObject>();
        private GameObject buildingParent => buildingParents[m_roadCreator.currentRoadIndex];
        public Dictionary<Vector3, Vector3> leftRoadVert { get; private set; }
        public Dictionary<Vector3, Vector3> rightRoadVert { get; private set; }

        private void OnValidate()
        {
            if (m_roadMeshCreator == null)
                m_roadMeshCreator = GetComponent<RoadMeshCreator>();
            if (m_roadCreator == null)
                m_roadCreator = GetComponent<RoadCreator>();
        }

        private int CalulatedBuildingSpacing => buildingSpacing * 2;
        private int CalulatedBuildingHeadAndTail => CalulatedBuildingSpacing * buildingRemoveHeadNum;

        public void UpdateBuilding()
        {
            m_roadMeshCreator.UpdateRoad();
            CreateVertexList(m_roadMeshCreator.verts,m_roadMeshCreator.segments);

            if (buildingParents.Count == 0)
            {
                GameObject buildingGO = new GameObject("BuildingParent");
                buildingGO.transform.SetParent(m_roadMeshCreator.currentRoadMeshObject.transform);
                if (!buildingParents.Contains(buildingGO))
                    buildingParents.Add(buildingGO);

                CreateBuilding(leftRoadVert);
                CreateBuilding(rightRoadVert);


            }
            else if (buildingParents.Count - 1 < m_roadCreator.currentRoadIndex)
            {
                GameObject buildingGO = new GameObject("BuildingParent");
                buildingGO.transform.SetParent(m_roadMeshCreator.currentRoadMeshObject.transform);
                if (!buildingParents.Contains(buildingGO))
                    buildingParents.Add(buildingGO);

                CreateBuilding(leftRoadVert);
                CreateBuilding(rightRoadVert);
            }

            ClearBuildings();
            CreateBuilding(leftRoadVert);
            CreateBuilding(rightRoadVert);
        }

        public void CreateBuilding(Dictionary<Vector3, Vector3> _vertexPoints)
        {
            //RemoveBuildingInCurveCorner(_vertexPoints);

            for (int i = 0; i < _vertexPoints.Count; i++)
            {
                for (int j = 0; j < buildingLayer; j++)
                {
                    BuildingInitCreator go = Instantiate(PF_proceduralBuilding, _vertexPoints.Keys.ElementAt(i), Quaternion.identity);
                    go.transform.SetParent(buildingParent.transform);
                    if (IsBuildingSizeBasedOnRoadSpacing)
                        go.RandomGenerateBuilding(m_roadMeshCreator.spacing);
                    else {
                        go.RandomGenerateBuilding(buildingSize);
                    }
                    go.transform.AngleY_PointOutward(go.transform.position, _vertexPoints[_vertexPoints.Keys.ElementAt(i)], true);
                    Vector3 dir = go.transform.position - _vertexPoints[_vertexPoints.Keys.ElementAt(i)];

                    go.transform.position += dir * buildinfOffsetFromRoad + dir * j * buildingLayerSpacing;
                    go.name = "Building_" + i;
                }
            }
        }

        private void RemoveBuildingInCurveCorner(Dictionary<Vector3, Vector3> _vertexPoints)
        {
            Dictionary<Vector3, Vector3> tmpList = _vertexPoints;
            for (int i = 0; i < tmpList.Count; i++)
            {
                if (i < tmpList.Count - 2)
                {
                    float dst = Vector3.Distance(tmpList.Keys.ElementAt(i), tmpList.Keys.ElementAt(i + 1));
                    //If Distance is less than spacing distance
                    if (dst > 11)
                    {
                        continue;
                    }
                    else
                    {
                        Vector3 dirToAngle = tmpList.Keys.ElementAt(i + 2) - tmpList.Keys.ElementAt(i);
                        float angle = Vector3.Angle(tmpList.Keys.ElementAt(i), dirToAngle);
                        float horizontalAngle = 180;
                        if (angle < horizontalAngle && angle > horizontalAngle - angleToRemoveBuilding)
                        {
                            continue;
                        }
                        else
                        {
                            //Remove Between point
                            _vertexPoints.Remove(tmpList.Keys.ElementAt(i + 1));
                        }
                    }
                }
            }
        }
        private void CreateVertexList(Vector3[] _vertsTotal, Vector3[] _segments)
        {
            if (_vertsTotal.Length <= 0)
                return;

            leftRoadVert = new Dictionary<Vector3, Vector3>();
            rightRoadVert = new Dictionary<Vector3, Vector3>();

            for (int i = CalulatedBuildingHeadAndTail; i < _vertsTotal.Length - CalulatedBuildingHeadAndTail; i += CalulatedBuildingSpacing)
            {
                leftRoadVert.Add(_vertsTotal[i], _segments[i / 2]);
            }
            for (int i = (1 + CalulatedBuildingHeadAndTail); i < _vertsTotal.Length - CalulatedBuildingHeadAndTail; i += CalulatedBuildingSpacing)
            {
                rightRoadVert.Add(_vertsTotal[i], _segments[i / 2]);
            }
        }

        public void ClearBuildings()
        {
            if (buildingParent == null)
                return;
            if (buildingParent.transform.childCount == 0)
                return;

            while (buildingParent.transform.childCount > 0)
            {
                DestroyImmediate(buildingParent.transform.GetChild(0).gameObject);
            }
        }

        public void RemoveRoad(int _index) {
            if(buildingParents.Count > _index)
                buildingParents.RemoveAt(_index);
        }
    }
}

