using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EditorTool;

[CustomEditor(typeof(RoadCreator))]
public class RoadEditor : Editor
{

    RoadCreator m_roadCreator;
    Road CurrentPath
    {
        get
        {
            return m_roadCreator.Path;
        }
    }

    const float segmentSelectDistanceThreshold = .1f;
    int selectedSegmentIndex = -1;

    //Intersection
    int intersectionIndex = -1;
    int selectedLinkPointIndex = -1;
    Intersection CurrentIntersection
    {
        get
        {
            if (intersectionIndex != -1 && m_roadCreator.interescetions.Count > 0)
            {
                return m_roadCreator.interescetions[intersectionIndex];
            }
            return null;
        }
    }

    //Create road instance
    bool isHoldingRoadObject = false;


    [MenuItem("Custom Tools/Add RoadCreator")]
    public static void Create()
    {
        GameObject GO_Creator = new GameObject("PathCreator", typeof(RoadCreator), typeof(RoadMeshCreator));
        GO_Creator.transform.position = Vector3.zero;
        RoadCreator roadCreator = GO_Creator.GetComponent<RoadCreator>();
        roadCreator.CreatePath();
    }


    public override void OnInspectorGUI()
    {
        m_roadCreator.showPrefabPropetry = EditorGUILayout.Foldout(m_roadCreator.showPrefabPropetry, "Show Prefab Slots");
        if (m_roadCreator.showPrefabPropetry)
        {
            m_roadCreator.pf_Intersectoin = (Intersection)EditorGUILayout.ObjectField("PF_intersection", m_roadCreator.pf_Intersectoin, typeof(Intersection), false);
            m_roadCreator.pf_Roundabout = (Roundabout)EditorGUILayout.ObjectField("PF_Roundabout", m_roadCreator.pf_Roundabout, typeof(Roundabout), false);
            EditorGUILayout.Space(15);
        }

        if (m_roadCreator.pf_Intersectoin == null)
        {
            EditorGUILayout.HelpBox("Assign Prefab accordingly", MessageType.Warning);
        }

        m_roadCreator.showControlPropetry = EditorGUILayout.Foldout(m_roadCreator.showControlPropetry, "Show Control propetries");
        if (m_roadCreator.showControlPropetry)
        {
            m_roadCreator.anchorColor = EditorGUILayout.ColorField("AnchorColor", m_roadCreator.anchorColor);
            m_roadCreator.controlColor = EditorGUILayout.ColorField("ControlColor", m_roadCreator.controlColor);
            m_roadCreator.segmentColor = EditorGUILayout.ColorField("SegmentColor", m_roadCreator.segmentColor);
            m_roadCreator.selectedSegmentColor = EditorGUILayout.ColorField("SelectedSegmentColor", m_roadCreator.selectedSegmentColor);
            m_roadCreator.anchorSize = EditorGUILayout.FloatField("AnchorSize", m_roadCreator.anchorSize);
            m_roadCreator.controlSize = EditorGUILayout.FloatField("ControlSize", m_roadCreator.controlSize);
            m_roadCreator.intersectionSize = EditorGUILayout.FloatField("IntersectionSize", m_roadCreator.intersectionSize);
        }

        EditorGUILayout.Space(25);
        EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginChangeCheck();
        GUI.enabled = !isHoldingRoadObject;
        if (GUILayout.Button("Select Intersection"))
        {
            m_roadCreator.SelectIntersection();
            isHoldingRoadObject = true;
        }
        if (GUILayout.Button("Select Roundabout"))
        {
            m_roadCreator.SelectRoundAbout();
            isHoldingRoadObject = true;
        }
        GUI.enabled = true;
        EditorGUILayout.EndHorizontal();

        bool isClosed = GUILayout.Toggle(CurrentPath.IsClosed, "Closed");
        if (isClosed != CurrentPath.IsClosed)
        {
            Undo.RecordObject(m_roadCreator, "Toggle closed");
            CurrentPath.IsClosed = isClosed;
        }

        bool autoSetControlPoints = GUILayout.Toggle(CurrentPath.AutoSetControlPoints, "Auto Set Control Points");
        if (autoSetControlPoints != CurrentPath.AutoSetControlPoints)
        {
            Undo.RecordObject(m_roadCreator, "Toggle auto set controls");
            CurrentPath.AutoSetControlPoints = autoSetControlPoints;
        }
        if (EditorGUI.EndChangeCheck())
        {
            SceneView.RepaintAll();
        }

        EditorGUILayout.Space(25);

        m_roadCreator.canShowPathList = EditorGUILayout.Foldout(m_roadCreator.canShowPathList, "Show Path List");
        if (m_roadCreator.canShowPathList)
        {
            for (int i = 0; i < m_roadCreator.paths.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUI.enabled = i != m_roadCreator.currentRoadIndex;
                GUILayout.Label("Path " + (i + 1));
                if (GUILayout.Button("Select"))
                {
                    m_roadCreator.currentRoadIndex = i;
                }
                GUI.enabled = true;

                GUI.enabled = m_roadCreator.paths.Count > 1;
                if (GUILayout.Button("Delete"))
                {
                    m_roadCreator.RemoveRoad(i);
                    m_roadCreator.currentRoadIndex = i == 0 ? 0 : m_roadCreator.currentRoadIndex -= 1;
                }
                GUILayout.EndHorizontal();
            }
        }
    }

    void OnSceneGUI()
    {
        Input();
        Draw();
    }

    void Input()
    {
        Event guiEvent = Event.current;
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
        float drawPlaneHeight = 0;
        float dstToDrawPlane = (drawPlaneHeight - mouseRay.origin.y) / mouseRay.direction.y;
        Vector3 mousePosition = mouseRay.GetPoint(dstToDrawPlane);

        //Shift + Left Click
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift && m_roadCreator.type.Equals(RoadCreator.HoldingType.Road))
        {
            if (selectedSegmentIndex != -1)
            {
                Undo.RecordObject(m_roadCreator, "Split segment");
                CurrentPath.SplitSegment(mousePosition, selectedSegmentIndex);
            }
            else if (!CurrentPath.IsClosed)
            {
                Undo.RecordObject(m_roadCreator, "Add segment");
                CurrentPath.AddSegment(mousePosition);
            }

            if (selectedLinkPointIndex != -1)
            {
                //Link Rrad To Intersection
            }
        }

        //Ctrl + Left Click
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.control)
        {
            switch (m_roadCreator.type)
            {
                case RoadCreator.HoldingType.Road:
                    m_roadCreator.AddPath(mousePosition);
                    HandleUtility.Repaint();
                    break;
                case RoadCreator.HoldingType.Intersection:
                    Selection.activeObject = m_roadCreator.AddIntersection(mousePosition);
                    break;
                case RoadCreator.HoldingType.Roundabout:
                    Selection.activeObject = m_roadCreator.AddRoundabout(mousePosition);
                    break;
            }
            isHoldingRoadObject = false;
        }

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.alt)
        {
            m_roadCreator.ReCenterPath(mousePosition);
        }


        //Shift + Right Click
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 1 && guiEvent.shift)
        {
            float minDstToAnchor = m_roadCreator.anchorSize * .5f;
            int closestAnchorIndex = -1;

            for (int i = 0; i < CurrentPath.NumPoints; i += 3)
            {
                float dst = Vector3.Distance(mousePosition, CurrentPath[i]);
                if (dst < minDstToAnchor)
                {
                    minDstToAnchor = dst;
                    closestAnchorIndex = i;
                }
            }

            if (closestAnchorIndex != -1)
            {
                Undo.RecordObject(m_roadCreator, "Delete segment");
                CurrentPath.DeleteSegment(closestAnchorIndex);
            }
        }

        if (guiEvent.type == EventType.MouseMove)
        {
            float minDstToSegment = segmentSelectDistanceThreshold;
            int newSelectedSegmentIndex = -1;

            for (int i = 0; i < CurrentPath.NumSegments; i++)
            {
                Vector3[] points = CurrentPath.GetPointsInSegment(i);
                float dst = HandleUtility.DistancePointBezier(mousePosition, points[0], points[3], points[1], points[2]);
                if (dst < minDstToSegment)
                {
                    minDstToSegment = dst;
                    newSelectedSegmentIndex = i;
                }
            }

            if (newSelectedSegmentIndex != selectedSegmentIndex)
            {
                selectedSegmentIndex = newSelectedSegmentIndex;
                HandleUtility.Repaint();
            }

            //TODO : Selected Intersection first, then detect which point is selected.
            int newSelectedLinkPointIndex = -1;
            for (int i = 0; i < m_roadCreator.interescetions.Count; i++)
            {
                Intersection _intersection = m_roadCreator.interescetions[i];
                for (int _intersectionPoints = 0; _intersectionPoints < _intersection.linkPoints.Length; _intersectionPoints++)
                {
                    newSelectedLinkPointIndex = _intersectionPoints;
                }
            }
            if (newSelectedLinkPointIndex != selectedLinkPointIndex)
            {
                selectedLinkPointIndex = newSelectedLinkPointIndex;
            }
        }

        HandleUtility.AddDefaultControl(0);
    }

    void Draw()
    {
        DrawRoad();

        for (int i = 0; i < m_roadCreator.interescetions.Count; i++)
        {
            Intersection _intersection = m_roadCreator.interescetions[i];
            for (int _intersectionPoints = 0; _intersectionPoints < _intersection.linkPoints.Length; _intersectionPoints++)
            {

                Handles.color = (selectedLinkPointIndex == i) ? Color.cyan : Color.gray;
                Handles.DrawSolidDisc(_intersection.linkPoints[_intersectionPoints], Vector3.up, m_roadCreator.intersectionSize);
            }
        }

        Tools.hidden = !m_roadCreator.displayTransformTool;
    }

    private void DrawRoad()
    {
        for (int _pathIndex = 0; _pathIndex < m_roadCreator.paths.Count; _pathIndex++)
        {
            Road Path = m_roadCreator.paths[_pathIndex];

            if (m_roadCreator.currentRoadIndex == _pathIndex)
            {
                for (int _segements = 0; _segements < Path.NumSegments; _segements++)
                {
                    Vector3[] points = Path.GetPointsInSegment(_segements);
                    if (m_roadCreator.displayControlPoints)
                    {
                        //Control line color
                        Handles.color = Color.black;
                        Handles.DrawLine(points[1], points[0]);
                        Handles.DrawLine(points[2], points[3]);
                    }
                    Color segmentCol = (_segements == selectedSegmentIndex && Event.current.shift) ? m_roadCreator.selectedSegmentColor : m_roadCreator.segmentColor;
                    Handles.DrawBezier(points[0], points[3], points[1], points[2], segmentCol, null, 2);
                }

                for (int _pathPoints = 0; _pathPoints < Path.NumPoints; _pathPoints++)
                {
                    if (_pathPoints % 3 == 0 || m_roadCreator.displayControlPoints)
                    {
                        Handles.color = (_pathPoints % 3 == 0) ? m_roadCreator.anchorColor : m_roadCreator.controlColor;
                        float handleSize = (_pathPoints % 3 == 0) ? m_roadCreator.anchorSize : m_roadCreator.controlSize;
                        Vector3 newPos = Handles.FreeMoveHandle(Path[_pathPoints], Quaternion.identity, handleSize, Vector3.zero, Handles.CylinderHandleCap);
                        if (Path[_pathPoints] != newPos)
                        {
                            Undo.RecordObject(m_roadCreator, "Move point");
                            Path.MovePoint(_pathPoints, newPos);
                        }
                    }
                }
            }
        }
    }

    void OnEnable()
    {
        m_roadCreator = (RoadCreator)target;
        Tools.hidden = !m_roadCreator.displayTransformTool;
    }

    private void OnDisable()
    {
        Tools.hidden = false;
    }
}
