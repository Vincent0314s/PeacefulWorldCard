using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static Vector2 ToXZ(this Vector3 _v3)
    {
        return new Vector2(_v3.x, _v3.z);
    }

    public static float SqrDistance(Vector3 pos1, Vector3 pos2)
    {
        Vector3 dir = pos1 - pos2;
        return Vector3.SqrMagnitude(dir);
    }
    public static void LookAtTargetXZ(this Transform trans, Vector3 targetPos)
    {
        Vector3 XZPos = new Vector3(targetPos.x, trans.position.y, targetPos.z);
        trans.LookAt(XZPos);
    }

    public static void AngleY_PointOutward(this Transform _trans, Vector3 v1, Vector3 v2, bool _isKeepingXZ = false)
    {
        float _r = Mathf.Atan2(v1.x - v2.x, v1.z - v2.z);
        float _d = _r * Mathf.Rad2Deg;
        Quaternion _targetRot;
        if (!_isKeepingXZ)
            _targetRot = Quaternion.Euler(0, _d, 0);
        else
            _targetRot = Quaternion.Euler(_trans.eulerAngles.x, _d, _trans.eulerAngles.z);

        _trans.rotation = _targetRot;
    }

    public static void AngleY_PointInward(this Transform _trans, Vector3 v1, Vector3 v2, bool _isKeepingXZ = false)
    {
        float _r = Mathf.Atan2(v1.x - v2.x, v1.z - v2.z);
        float _d = _r * Mathf.Rad2Deg + 180;
        Quaternion _targetRot = Quaternion.Euler(0, _d, 0);
        if (!_isKeepingXZ)
            _targetRot = Quaternion.Euler(0, _d, 0);
        else
            _targetRot = Quaternion.Euler(_trans.eulerAngles.x, _d, _trans.eulerAngles.z);

        _trans.rotation = _targetRot;
    }

    public static Mesh CreateRoadMesh(Vector3[] points, float _roadWidth, bool isClosed)
    {
        Vector3[] verts = new Vector3[points.Length * 2];
        Vector2[] uvs = new Vector2[verts.Length];
        int numTris = 2 * (points.Length - 1) + ((isClosed) ? 2 : 0);
        int[] tris = new int[numTris * 3];
        int vertIndex = 0;
        int triIndex = 0;

        for (int i = 0; i < points.Length; i++)
        {
            Vector3 forward = Vector3.zero;
            if (i < points.Length - 1 || isClosed)
            {
                forward += points[(i + 1) % points.Length] - points[i];
            }
            if (i > 0 || isClosed)
            {
                forward += points[i] - points[(i - 1 + points.Length) % points.Length];
            }

            forward.Normalize();
            Vector3 left = new Vector3(-forward.z, 0, forward.x);

            verts[vertIndex] = points[i] + left * _roadWidth * .5f;
            verts[vertIndex + 1] = points[i] - left * _roadWidth * .5f;


            float completionPercent = i / (float)(points.Length - 1);
            float v = 1 - Mathf.Abs(2 * completionPercent - 1);
            uvs[vertIndex] = new Vector2(0, v);
            uvs[vertIndex + 1] = new Vector2(1, v);

            if (i < points.Length - 1 || isClosed)
            {
                tris[triIndex] = vertIndex;
                tris[triIndex + 1] = (vertIndex + 2) % verts.Length;
                tris[triIndex + 2] = vertIndex + 1;

                tris[triIndex + 3] = vertIndex + 1;
                tris[triIndex + 4] = (vertIndex + 2) % verts.Length;
                tris[triIndex + 5] = (vertIndex + 3) % verts.Length;
            }

            vertIndex += 2;
            triIndex += 6;
        }


        Mesh mesh = new Mesh();
        mesh.Clear();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        return mesh;
    }

}
