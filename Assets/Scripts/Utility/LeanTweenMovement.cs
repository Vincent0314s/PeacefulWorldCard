using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanTweenMovement : MonoBehaviour
{
    [SerializeField] private Vector3 _destination;
    [SerializeField] private float _movingDuration;
    [SerializeField] private AnimationCurve _movingCurve;

    private Vector3 _originalPosition;

    private void Start()
    {
        _originalPosition = transform.localPosition;
    }

    public void MoveToDestination()
    {
        LeanTween.moveLocal(gameObject, _destination, _movingDuration).setEase(_movingCurve);
    }

    public void ResetPosition()
    {
        transform.localPosition = _originalPosition;
    }
}
