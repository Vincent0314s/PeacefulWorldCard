using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Roundabout : MonoBehaviour
{
    public int smoothness = 10;
    public float radius = 1f;
    public float roadWidth = 1f;
    private MeshFilter mf;
    private List<Vector3> points = new List<Vector3>();

    public void UpdateRoundabout() {
        CreateRoundabout();
    }

    public void CreateRoundabout()
    {
        points.Clear();
        if (mf == null)
            mf = GetComponent<MeshFilter>();

        for (int i = 0; i < smoothness; i++)
        {
            float theta = i * 2 * Mathf.PI / smoothness;
            float x = Mathf.Sin(theta) * radius;
            float y = Mathf.Cos(theta) * radius;

            Vector3 circlePoint = new Vector3(x, 0, y);
            points.Add(circlePoint);
        }
        mf.sharedMesh = ExtensionMethods.CreateRoadMesh(points.ToArray(), roadWidth, true);
    }
}
