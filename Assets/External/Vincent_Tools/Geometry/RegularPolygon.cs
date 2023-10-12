using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer)),ExecuteAlways]
public class RegularPolygon : MonoBehaviour
{
    public Material material;
    [Min(0)]
    public float radius = 5f;
    [Range(3,30)]
    public int numVertices = 5;
    [Range(0,360)]
    public int startingAngle = 0;

    private MeshFilter meshFiliter;
    private MeshRenderer meshRender;
    private Mesh mesh;
    private void OnValidate()
    {
        if (mesh != null)
            Start();
    }

    private void Awake()
    {
        TryGetComponent<MeshFilter>(out meshFiliter);
        TryGetComponent<MeshRenderer>(out meshRender);
        mesh = new Mesh();
        meshFiliter.mesh = mesh;
    }
    private void Start()
    {
        mesh.Clear();
        meshRender.material = material;

        // Angle of each segment in radians.
        float angle = 2 * Mathf.PI / numVertices;

        //Create vertices around the polygon.
        Vector3[] vertices = new Vector3[numVertices];
        Vector2[] uv = new Vector2[numVertices];

        float startingRadins = Mathf.Deg2Rad * startingAngle;

        for (int i = 0; i < numVertices; i++)
        {
            vertices[i] = new Vector3(Mathf.Sin(startingRadins + (i * angle)),0,Mathf.Cos(startingRadins + (i * angle))) * radius;
            uv[i] = new Vector2(1 + Mathf.Sin(startingRadins + (i * angle)), 1 + Mathf.Cos(startingRadins + (i * angle))) * 0.5f;
        }
        mesh.vertices = vertices;
        mesh.uv = uv;

        //The triangle vertices must be done in clockwise order.
        int[] triangles = new int[3*(numVertices - 2)];
        for (int i = 0; i < numVertices - 2; i++)
        {
            triangles[3 * i] = 0;
            triangles[(3 * i) + 1] = i + 1;
            triangles[(3 * i) + 2] = i + 2;
        }
        mesh.triangles = triangles;
    }
}
