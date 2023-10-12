using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EditorTool
{
    public class RoadCreator : MonoBehaviour
    {
        public enum HoldingType
        {
            Road,
            Intersection,
            Roundabout,
        }

        public HoldingType type;

        public bool showPrefabPropetry;
        public Intersection pf_Intersectoin;
        public Roundabout pf_Roundabout;
        public Road Path => paths[currentRoadIndex];

        public static Action<int> OnDeletedRoad;

        public bool showControlPropetry;
        public Color anchorColor = Color.red;
        public Color controlColor = Color.white;
        public Color segmentColor = Color.green;
        public Color selectedSegmentColor = Color.yellow;
        public float anchorSize = .1f;
        public float controlSize = .075f;
        public float intersectionSize = 0.5f;
        public bool displayControlPoints = true;
        public bool displayTransformTool = false;

        public bool canShowPathList;
        public List<Road> paths = new List<Road>();
        public int currentRoadIndex;

        private GameObject m_intersectionParent, m_roundAboutParent;
        public List<Intersection> interescetions = new List<Intersection>();


        public void CreatePath()
        {
            paths.Add(new Road(transform.position));
            currentRoadIndex = paths.Count - 1;
        }

        public void AddPath(Vector3 _mousePosition)
        {
            paths.Add(new Road(_mousePosition));
            currentRoadIndex = paths.Count - 1;
        }

        public void ReCenterPath(Vector3 _mousePosition)
        {
            paths[currentRoadIndex] = new Road(_mousePosition);
        }

        public void SelectIntersection()
        {
            type = HoldingType.Intersection;
        }

        public void SelectRoundAbout()
        {
            type = HoldingType.Roundabout;
        }

        public GameObject AddIntersection(Vector3 _mousePosition)
        {
            GameObject go = Instantiate(pf_Intersectoin.gameObject, _mousePosition, Quaternion.identity);
            if (m_intersectionParent == null) { 
                m_intersectionParent = new GameObject("IntersectionColliection");
                m_intersectionParent.transform.SetParent(transform,false);
            }

            go.transform.SetParent(m_intersectionParent.transform, false);
            Intersection _intersection = go.GetComponent<Intersection>();
            if(!interescetions.Contains(_intersection))
                interescetions.Add(_intersection);

            Debug.Log(interescetions.Count);
            type = HoldingType.Road;
            return go;
        }

        public GameObject AddRoundabout(Vector3 _mousePosition)
        {
            GameObject go = Instantiate(pf_Roundabout.gameObject, _mousePosition, Quaternion.identity);
            if (m_roundAboutParent == null) { 
                m_roundAboutParent = new GameObject("RoundaboutColliection");
                m_roundAboutParent.transform.SetParent(transform,false);
            }

            go.transform.SetParent(m_roundAboutParent.transform, false);
            type = HoldingType.Road;
            return go;
        }

        public void RemoveRoad(int _index)
        {
            OnDeletedRoad?.Invoke(_index);
            paths.RemoveAt(_index);

        }
    }
}
