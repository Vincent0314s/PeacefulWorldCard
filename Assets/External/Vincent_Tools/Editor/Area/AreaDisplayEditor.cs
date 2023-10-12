using UnityEngine;
using UnityEditor;
using Vincent.EditorTool;
using System;

[CustomEditor(typeof(AreaDisplayCreator))]
public class AreaDisplayEditor : Editor
{
    public class SelectionInfo
    {
        public int pointIndex = -1;
        public bool mouseIsOverPoint;
        public bool pointIsSelected;
        public Vector3 positionAtStartOfDrag;

        public int lineIndex = -1;
        public bool mouseIsOverLine;
    }

    private AreaDisplayCreator m_areaCreator;
    private SelectionInfo m_selectionInfo;
    private bool rePaintShapeSinceLastChanged;

    private float handleRadius = 0.5f;

    #region Unity Init Methods
    [MenuItem("Custom Tools/Add AreaCreator")]
    public static void Create()
    {
        GameObject GO_Creator = new GameObject(StringDefs.areaCreatorName, typeof(AreaDisplayCreator));
        GO_Creator.transform.position = Vector3.zero;
    }
    private void OnEnable()
    {
        rePaintShapeSinceLastChanged = true;
        m_areaCreator = target as AreaDisplayCreator;
        m_selectionInfo = new SelectionInfo();
        Tools.hidden = true;
    }

    void OnDisable()
    {
        Tools.hidden = false;
    }
    #endregion

    #region GUI and GUIEvent
    public override void OnInspectorGUI()
    {
        m_areaCreator.meshMaterial = EditorGUILayout.ObjectField("Mesh Material",m_areaCreator.meshMaterial,typeof(Material),false) as Material;
        if (m_areaCreator.meshMaterial == null)
        {
            string helpMessage = "Assign Material before creating mesh.";
            EditorGUILayout.HelpBox(helpMessage, MessageType.Warning);
        }

        handleRadius = EditorGUILayout.FloatField("Handle Radius",handleRadius);
        m_areaCreator.areaHeight = EditorGUILayout.FloatField("Area Height",m_areaCreator.areaHeight);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("areaList"),true);

    

        GUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("Create Mesh", "ShortCut : Shift+E")))
        {
            m_areaCreator.UpdateMeshDisplay();
            m_areaCreator.ResetPoints();
        }

        if (GUILayout.Button(new GUIContent("Reset Points", "ShortCut : Shift+R")))
        {
            m_areaCreator.ResetPoints();
        }
        if (GUILayout.Button("Reset Area Index"))
        {
            m_areaCreator.ResetAreaIndex();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Refresh Area List"))
        {
            m_areaCreator.RemoveNullAreaInList();
        }
        if (GUILayout.Button("Clear Area"))
        {
            m_areaCreator.ClearArea();
        }
        if (GUILayout.Button("Rebuild Area"))
        {
            m_areaCreator.RebuildArea();
        }
        GUILayout.EndHorizontal();

        m_areaCreator.canAddNameForArea = EditorGUILayout.Toggle("Add Name For Next Area", m_areaCreator.canAddNameForArea);
        if (m_areaCreator.canAddNameForArea)
        {
            m_areaCreator.nameForArea = EditorGUILayout.TextField("Area Name", m_areaCreator.nameForArea);
        }

        m_areaCreator.canAddComponentForArea = EditorGUILayout.Toggle("Add Component For All Area", m_areaCreator.canAddComponentForArea);
        if (m_areaCreator.canAddComponentForArea)
        {
            m_areaCreator.script = EditorGUILayout.ObjectField(m_areaCreator.script, typeof(TextAsset), false) as TextAsset;

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Component", GUILayout.Width(150), GUILayout.Height(25)))
            {
                AddComponentForArea();
            }
            if (GUILayout.Button("Remove Component", GUILayout.Width(150), GUILayout.Height(25)))
            {
                RemoveComponentForArea();
            }
            GUILayout.EndHorizontal();
        }
       

        if (GUI.changed)
        {
            rePaintShapeSinceLastChanged = true;
            SceneView.RepaintAll();
        }
    }

    private void OnSceneGUI()
    {
        Event guiEvent = Event.current;

        if (guiEvent.type == EventType.Repaint)
        {
            Draw();
        }
        else if (guiEvent.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        }
        else
        {
            Input(guiEvent);
            if (rePaintShapeSinceLastChanged)
            {
                HandleUtility.Repaint();
            }
        }
    }
    private void Input(Event _guiEvent)
    {
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(_guiEvent.mousePosition);
        float dstToDrawPlane = (m_areaCreator.areaHeight - mouseRay.origin.y) / mouseRay.direction.y;
        Vector3 mousePosition = mouseRay.GetPoint(dstToDrawPlane);

        switch (_guiEvent.type)
        {
            case EventType.KeyDown:
                {
                    if (_guiEvent.modifiers == EventModifiers.Shift)
                    {
                        if (Event.current.keyCode == KeyCode.E)
                        {
                            m_areaCreator.UpdateMeshDisplay();
                            m_areaCreator.ResetPoints();
                        }
                        else if (Event.current.keyCode == KeyCode.R)
                        {
                            m_areaCreator.ResetPoints();

                        }
                    }
                    break;
                }
            case EventType.MouseDown:
                if (_guiEvent.button == 0)
                {
                    if (_guiEvent.modifiers == EventModifiers.Shift)
                    {
                        HandleShiftLeftMouseDown(mousePosition);
                    }
                    else if (_guiEvent.modifiers == EventModifiers.None)
                    {
                        HandleLeftMouseDown(mousePosition);
                    }
                }
                break;
            case EventType.MouseUp:
                if (_guiEvent.button == 0)
                {
                    HandleLeftMouseUp(mousePosition);
                }
                break;
            case EventType.MouseDrag:
                if (_guiEvent.button == 0 && _guiEvent.modifiers == EventModifiers.None)
                {
                    HandleLeftMouseDrag(mousePosition);
                }
                break;
        }

        #region Need to abandon this once we get stable version
        //if (_guiEvent.type == EventType.MouseDown && _guiEvent.button == 0 && _guiEvent.modifiers == EventModifiers.Shift)
        //{
        //    HandleShiftLeftMouseDown(mousePosition);
        //}

        //if (_guiEvent.type == EventType.MouseDown && _guiEvent.button == 0 && _guiEvent.modifiers == EventModifiers.None)
        //{
        //    HandleLeftMouseDown(mousePosition);
        //}
        //if (_guiEvent.type == EventType.MouseUp && _guiEvent.button == 0)
        //{
        //    HandleLeftMouseUp(mousePosition);
        //}
        //if (_guiEvent.type == EventType.MouseDrag && _guiEvent.button == 0 && _guiEvent.modifiers == EventModifiers.None)
        //{
        //    HandleLeftMouseDrag(mousePosition);
        //}
        #endregion

        if (!m_selectionInfo.pointIsSelected)
            UpdateMouseOverSelection(mousePosition);
    }
    #endregion

    #region Draw Points
    private void CreateNewPoint(Vector3 _mousePosition)
    {
        int newPointIndex = (m_selectionInfo.mouseIsOverLine) ? m_selectionInfo.lineIndex + 1 : m_areaCreator.points.Count;
        Undo.RecordObject(m_areaCreator, "Add Point");
        m_areaCreator.points.Insert(newPointIndex, _mousePosition);
        m_selectionInfo.pointIndex = newPointIndex;

        rePaintShapeSinceLastChanged = true;
        SelectePointUnderMouse();
    }

    private void DeletedPointUnderMouse()
    {
        Undo.RecordObject(m_areaCreator, "Delete Point");
        m_areaCreator.points.RemoveAt(m_selectionInfo.pointIndex);
        m_selectionInfo.pointIsSelected = false;
        m_selectionInfo.mouseIsOverPoint = false;
        rePaintShapeSinceLastChanged = true;
    }

    private void SelectePointUnderMouse()
    {
        m_selectionInfo.pointIsSelected = true;
        m_selectionInfo.mouseIsOverPoint = true;
        m_selectionInfo.mouseIsOverLine = false;
        m_selectionInfo.lineIndex = -1;

        //m_selectionInfo.positionAtStartOfDrag = SelectedShape.points[m_selectionInfo.pointIndex];
        rePaintShapeSinceLastChanged = true;
    }



    private void HandleShiftLeftMouseDown(Vector3 _mousePosition)
    {
        if (m_selectionInfo.mouseIsOverPoint)
        {
            DeletedPointUnderMouse();
        }
        #region Need to abandon this once we get stable version
        //else
        //{
        //    CreateNewPoint(_mousePosition);
        //}
        #endregion
    }

    private void HandleLeftMouseDown(Vector3 _mousePosition)
    {
        if (m_selectionInfo.mouseIsOverPoint)
        {
            SelectePointUnderMouse();
        }
        else
        {
            CreateNewPoint(_mousePosition);
        }
    }

    private void HandleLeftMouseUp(Vector3 _mousePosition)
    {
        if (m_selectionInfo.pointIsSelected)
        {
            m_areaCreator.points[m_selectionInfo.pointIndex] = m_selectionInfo.positionAtStartOfDrag;
            Undo.RecordObject(m_areaCreator, "Move Point");
            m_areaCreator.points[m_selectionInfo.pointIndex] = _mousePosition;
            m_selectionInfo.pointIndex = -1;
            m_selectionInfo.pointIsSelected = false;
            rePaintShapeSinceLastChanged = true;
        }
    }

    private void HandleLeftMouseDrag(Vector3 _mousePosition)
    {
        if (m_selectionInfo.pointIsSelected)
        {
            m_areaCreator.points[m_selectionInfo.pointIndex] = _mousePosition;
            rePaintShapeSinceLastChanged = true;
        }

    }

    private void UpdateMouseOverSelection(Vector3 _mousePosition)
    {
        int mouseOverPointIndex = -1;
        for (int i = 0; i < m_areaCreator.points.Count; i++)
        {
            if (Vector3.Distance(_mousePosition, m_areaCreator.points[i]) < handleRadius)
            {
                mouseOverPointIndex = i;
                break;
            }
        }

        if (mouseOverPointIndex != m_selectionInfo.pointIndex)
        {
            m_selectionInfo.pointIndex = mouseOverPointIndex;
            m_selectionInfo.mouseIsOverPoint = mouseOverPointIndex != -1;

            rePaintShapeSinceLastChanged = true;
        }

        if (m_selectionInfo.mouseIsOverPoint)
        {
            m_selectionInfo.mouseIsOverLine = false;
            m_selectionInfo.lineIndex = -1;
        }
        else
        {
            int mouseOverLineIndex = -1;
            float closestLineDst = handleRadius;
            for (int i = 0; i < m_areaCreator.points.Count; i++)
            {
                //TODO :
                Vector3 nextPointInLine = m_areaCreator.points[(i + 1) % m_areaCreator.points.Count];
                //TODO :
                float dstFromMouseToLine = HandleUtility.DistancePointToLineSegment(_mousePosition.ToXZ(), m_areaCreator.points[i].ToXZ(), nextPointInLine.ToXZ());

                if (dstFromMouseToLine < closestLineDst)
                {
                    closestLineDst = dstFromMouseToLine;
                    mouseOverLineIndex = i;
                }
            }

            if (m_selectionInfo.lineIndex != mouseOverLineIndex)
            {
                m_selectionInfo.lineIndex = mouseOverLineIndex;
                m_selectionInfo.mouseIsOverLine = mouseOverLineIndex != -1;
                rePaintShapeSinceLastChanged = true;
            }
        }
    }

    private void Draw()
    {
        for (int i = 0; i < m_areaCreator.points.Count; i++)
        {
            //TODO :
            Vector3 nextPoint = m_areaCreator.points[(i + 1) % m_areaCreator.points.Count];
            if (i == m_selectionInfo.lineIndex)
            {
                Handles.color = Color.red;
                Handles.DrawLine(m_areaCreator.points[i], nextPoint);
            }
            else
            {
                Handles.color = Color.black;
                Handles.DrawDottedLine(m_areaCreator.points[i], nextPoint, 4);
            }

            if (i == m_selectionInfo.pointIndex)
            {
                Handles.color = (m_selectionInfo.pointIsSelected) ? Color.black : Color.red;
            }
            else
            {
                Handles.color = Color.white;
            }

            Handles.DrawSolidDisc(m_areaCreator.points[i], Vector3.up, handleRadius);
        }

        //if (rePaintShapeSinceLastChanged) {
        //    m_shapeCreator.UpdateMeshDisplay();
        //}

        rePaintShapeSinceLastChanged = false;
    }
    #endregion

    #region Private Methods
    private void AddComponentForArea()
    {
        if (m_areaCreator.canAddComponentForArea)
        {
            if (m_areaCreator.areaList.Count <= 0)
            {
                return;
            }
            else
            {
                for (int i = 0; i < m_areaCreator.areaList.Count; i++)
                {
                    GameObject areaGO = m_areaCreator.areaList[i];
                    ObjectFactory.AddComponent(areaGO, System.Reflection.Assembly.Load("Assembly-CSharp").GetType(m_areaCreator.script.name));
                }
            }
        }
    }

    private void RemoveComponentForArea()
    {
        if (m_areaCreator.canAddComponentForArea)
        {
            if (m_areaCreator.areaList.Count <= 0)
            {
                return;
            }
            else
            {
                for (int i = 0; i < m_areaCreator.areaList.Count; i++)
                {
                    GameObject areaGO = m_areaCreator.areaList[i];
                    if (areaGO.TryGetComponent(System.Reflection.Assembly.Load("Assembly-CSharp").GetType(m_areaCreator.script.name), out Component component)) { 
                        DestroyImmediate(component);
                    }
                }
            }
        }
    }
    #endregion



}
