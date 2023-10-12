using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShapeCreator))]
public class ShapeEditor : Editor
{
    ShapeCreator m_shapeCreator;
    SelectionInfo m_selectionInfo;
    bool rePaintShapeSinceLastChanged;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        int shapeDeleteIndex = -1;
        m_shapeCreator.showShapeList = EditorGUILayout.Foldout(m_shapeCreator.showShapeList, "Show Shape List");
        if (m_shapeCreator.showShapeList)
        {
            for (int i = 0; i < m_shapeCreator.shapes.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUI.enabled = i != m_selectionInfo.selectedShapeIndex;
                GUILayout.Label("Shape " + (i + 1));
                if (GUILayout.Button("Select"))
                {
                    m_selectionInfo.selectedShapeIndex = i;
                }

                GUI.enabled = true;
                if (GUILayout.Button("Delete"))
                {
                    shapeDeleteIndex = i;
                }
                GUILayout.EndHorizontal();
            }
        }


        if (shapeDeleteIndex != -1)
        {
            Undo.RecordObject(m_shapeCreator, "Delete Shape");
            m_shapeCreator.shapes.RemoveAt(shapeDeleteIndex);
            m_selectionInfo.selectedShapeIndex = Mathf.Clamp(m_selectionInfo.selectedShapeIndex, 0, m_shapeCreator.shapes.Count - 1);
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

    private void CreateNewShape()
    {
        Undo.RecordObject(m_shapeCreator, "Create Shape");
        m_shapeCreator.shapes.Add(new Shape());
        m_selectionInfo.selectedShapeIndex = m_shapeCreator.shapes.Count - 1;
    }

    private void CreateNewPoint(Vector3 _mousePosition)
    {
        bool mouseIsOverSelectedShape = m_selectionInfo.mouseOverShapeIndex == m_selectionInfo.selectedShapeIndex;
        int newPointIndex = (m_selectionInfo.mouseIsOverLine && mouseIsOverSelectedShape) ? m_selectionInfo.lineIndex + 1 : SelectedShape.points.Count;
        Undo.RecordObject(m_shapeCreator, "Add Point");
        SelectedShape.points.Insert(newPointIndex, _mousePosition);
        m_selectionInfo.pointIndex = newPointIndex;
        m_selectionInfo.mouseOverShapeIndex = m_selectionInfo.selectedShapeIndex;

        rePaintShapeSinceLastChanged = true;
        SelectePointUnderMouse();
    }

    private void DeletedPointUnderMouse()
    {
        Undo.RecordObject(m_shapeCreator, "Delete Point");
        SelectedShape.points.RemoveAt(m_selectionInfo.pointIndex);
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

        m_selectionInfo.positionAtStartOfDrag = SelectedShape.points[m_selectionInfo.pointIndex];
        rePaintShapeSinceLastChanged = true;
    }

    private void SelectedShapeUnderMouse()
    {
        if (m_selectionInfo.mouseOverShapeIndex != -1)
        {
            m_selectionInfo.selectedShapeIndex = m_selectionInfo.mouseOverShapeIndex;
            rePaintShapeSinceLastChanged = true;
        }
    }

    private void Input(Event _guiEvent)
    {
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(_guiEvent.mousePosition);
        float drawPlaneHeight = 0;
        float dstToDrawPlane = (drawPlaneHeight - mouseRay.origin.y) / mouseRay.direction.y;
        Vector3 mousePosition = mouseRay.GetPoint(dstToDrawPlane);

        if (_guiEvent.type == EventType.MouseDown && _guiEvent.button == 0 && _guiEvent.modifiers == EventModifiers.Shift)
        {
            HandleShiftLeftMouseDown(mousePosition);
        }

        if (_guiEvent.type == EventType.MouseDown && _guiEvent.button == 0 && _guiEvent.modifiers == EventModifiers.None)
        {
            HandleLeftMouseDown(mousePosition);
        }
        if (_guiEvent.type == EventType.MouseUp && _guiEvent.button == 0)
        {
            HandleLeftMouseUp(mousePosition);
        }
        if (_guiEvent.type == EventType.MouseDrag && _guiEvent.button == 0 && _guiEvent.modifiers == EventModifiers.None)
        {
            HandleLeftMouseDrag(mousePosition);
        }
        if (!m_selectionInfo.pointIsSelected)
            UpdateMouseOverSelection(mousePosition);
    }

    private void HandleShiftLeftMouseDown(Vector3 _mousePosition)
    {
        if (m_selectionInfo.mouseIsOverPoint)
        {
            SelectedShapeUnderMouse();
            DeletedPointUnderMouse();
        }
        else
        {
            CreateNewShape();
            CreateNewPoint(_mousePosition);
        }
    }

    private void HandleLeftMouseDown(Vector3 _mousePosition)
    {
        if (m_shapeCreator.shapes.Count == 0)
        {
            CreateNewShape();
        }

        SelectedShapeUnderMouse();

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
            SelectedShape.points[m_selectionInfo.pointIndex] = m_selectionInfo.positionAtStartOfDrag;
            Undo.RecordObject(m_shapeCreator, "Move Point");
            SelectedShape.points[m_selectionInfo.pointIndex] = _mousePosition;
            m_selectionInfo.pointIndex = -1;
            rePaintShapeSinceLastChanged = true;
            m_selectionInfo.pointIsSelected = false;
        }
    }

    private void HandleLeftMouseDrag(Vector3 _mousePosition)
    {
        if (m_selectionInfo.pointIsSelected)
        {
            SelectedShape.points[m_selectionInfo.pointIndex] = _mousePosition;
            rePaintShapeSinceLastChanged = true;
        }

    }

    private void UpdateMouseOverSelection(Vector3 _mousePosition)
    {
        int mouseOverPointIndex = -1;
        int mouseOverShapeIndex = -1;
        for (int shapeIndex = 0; shapeIndex < m_shapeCreator.shapes.Count; shapeIndex++)
        {
            Shape currentShape = m_shapeCreator.shapes[shapeIndex];

            for (int i = 0; i < currentShape.points.Count; i++)
            {
                if (Vector3.Distance(_mousePosition, currentShape.points[i]) < m_shapeCreator.handleRadius)
                {
                    mouseOverPointIndex = i;
                    mouseOverShapeIndex = shapeIndex;
                    break;
                }
            }
        }

        if (mouseOverPointIndex != m_selectionInfo.pointIndex || mouseOverShapeIndex != m_selectionInfo.mouseOverShapeIndex)
        {
            m_selectionInfo.mouseOverShapeIndex = mouseOverShapeIndex;
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
            float closestLineDst = m_shapeCreator.handleRadius;
            for (int shapeIndex = 0; shapeIndex < m_shapeCreator.shapes.Count; shapeIndex++)
            {
                Shape currentShape = m_shapeCreator.shapes[shapeIndex];

                for (int i = 0; i < currentShape.points.Count; i++)
                {
                    //TODO :
                    Vector3 nextPointInLine = currentShape.points[(i + 1) % currentShape.points.Count];
                    //TODO :
                    float dstFromMouseToLine = HandleUtility.DistancePointToLineSegment(_mousePosition.ToXZ(), currentShape.points[i].ToXZ(), nextPointInLine.ToXZ());

                    if (dstFromMouseToLine < closestLineDst)
                    {
                        closestLineDst = dstFromMouseToLine;
                        mouseOverLineIndex = i;
                        mouseOverShapeIndex = shapeIndex;
                    }
                }
            }

            if (m_selectionInfo.lineIndex != mouseOverLineIndex || m_selectionInfo.mouseOverShapeIndex != mouseOverShapeIndex)
            {
                m_selectionInfo.mouseOverShapeIndex = mouseOverShapeIndex;
                m_selectionInfo.lineIndex = mouseOverLineIndex;
                m_selectionInfo.mouseIsOverLine = mouseOverLineIndex != -1;
                rePaintShapeSinceLastChanged = true;
            }
        }
    }

    private void Draw()
    {
        for (int shapeIndex = 0; shapeIndex < m_shapeCreator.shapes.Count; shapeIndex++)
        {
            Shape shapeToDraw = m_shapeCreator.shapes[shapeIndex];
            bool shapeIsSelected = shapeIndex == m_selectionInfo.selectedShapeIndex;
            bool mouseIsOverShape = shapeIndex == m_selectionInfo.mouseOverShapeIndex;
            Color deselectedShapeColor = Color.gray;

            for (int i = 0; i < shapeToDraw.points.Count; i++)
            {
                //TODO :
                Vector3 nextPoint = shapeToDraw.points[(i + 1) % shapeToDraw.points.Count];
                if (i == m_selectionInfo.lineIndex && mouseIsOverShape)
                {
                    Handles.color = Color.red;
                    Handles.DrawLine(shapeToDraw.points[i], nextPoint);
                }
                else
                {
                    Handles.color = (shapeIsSelected) ? Color.black : deselectedShapeColor;
                    Handles.DrawDottedLine(shapeToDraw.points[i], nextPoint, 4f);
                }

                if (i == m_selectionInfo.pointIndex && mouseIsOverShape)
                {
                    Handles.color = (m_selectionInfo.pointIsSelected) ? Color.black : Color.red;
                }
                else
                {
                    Handles.color = (shapeIsSelected) ? Color.white : deselectedShapeColor;
                }
                Handles.DrawSolidDisc(shapeToDraw.points[i], Vector3.up, m_shapeCreator.handleRadius);
            }
        }

        if (rePaintShapeSinceLastChanged) {
            m_shapeCreator.UpdateMeshDisplay();
        }

        rePaintShapeSinceLastChanged = false;
    }

    private void OnEnable()
    {
        rePaintShapeSinceLastChanged = true;
        m_shapeCreator = target as ShapeCreator;
        m_selectionInfo = new SelectionInfo();
        Undo.undoRedoPerformed += OnUndoOrRedo;
        Tools.hidden = true;
    }

    void OnDisable()
    {
        Undo.undoRedoPerformed -= OnUndoOrRedo;
        Tools.hidden = false;
    }

    private void OnUndoOrRedo()
    {
        if (m_selectionInfo.selectedShapeIndex >= m_shapeCreator.shapes.Count || m_selectionInfo.selectedShapeIndex == -1)
        {
            m_selectionInfo.selectedShapeIndex = m_shapeCreator.shapes.Count - 1;
        }
        rePaintShapeSinceLastChanged = true;
    }


    Shape SelectedShape => m_shapeCreator.shapes[m_selectionInfo.selectedShapeIndex];

    public class SelectionInfo
    {
        public int selectedShapeIndex;
        public int mouseOverShapeIndex;

        public int pointIndex = -1;
        public bool mouseIsOverPoint;
        public bool pointIsSelected;
        public Vector3 positionAtStartOfDrag;

        public int lineIndex = -1;
        public bool mouseIsOverLine;
    }
}
