using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolygonTool;

public class ShapeCreator : MonoBehaviour
{
    public MeshFilter meshFiliter;

    [HideInInspector]
    public List<Shape> shapes = new List<Shape>();
    [HideInInspector]
    public bool showShapeList;
    public float handleRadius = 0.5f;

    public void UpdateMeshDisplay() {
        //if(shapes.Count > 0)
        //for (int i = 0; i < shapes.Count; i++)
        //{
        //    if(shapes[i].points.Count > 3)
        //        meshFiliter.mesh = Triangulation.GenPolyMesh(shapes[i].points);
        //}
    }
}

[System.Serializable]
public class Shape {
    public List<Vector3> points = new List<Vector3>();
}
