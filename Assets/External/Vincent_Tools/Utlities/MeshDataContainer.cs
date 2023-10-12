using UnityEngine;
using PolygonTool;

[RequireComponent(typeof(MeshFilter))]
public class MeshDataContainer : MonoBehaviour
{
    [SerializeField] private Vector2[] uv;
    [SerializeField] private Vector3[] verticies;
    [SerializeField] private int[] triangles;

    public void SetMeshData(Mesh _mesh)
    {
        uv = _mesh.uv;
        verticies = _mesh.vertices;
        triangles = _mesh.triangles;
    }

    public void Rebuild()
    {
        GetComponent<MeshFilter>().sharedMesh = Triangulation.GenPolyMesh(verticies, triangles, uv);
    }
}

