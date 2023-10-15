using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    [SerializeField] private int _segementCount = 2;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        _lineRenderer.positionCount = _segementCount;
        ResetLine();
    }

    public void SetStartPoint(Vector3 startPosition)
    {
        _lineRenderer.SetPosition(0, startPosition);
    }

    public void SetTargetPoint(Vector3 targetPosition)
    {
        _lineRenderer.SetPosition(1, targetPosition);
    }

    public void CreateLine()
    {
        _lineRenderer.enabled = true;
    }

    public void ResetLine()
    {
        SetTargetPoint(new Vector3(0, 0, 0));
        _lineRenderer.enabled = false;
    }
}
