using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotationBehaviour : CharacterBehaviour
{
    public enum RotationSpeeds
    {
        Instant,
        Smooth,
        SmoothAbsolute
    }
    public RotationSpeeds MovementRotationSpeed = RotationSpeeds.Instant;
    public GameObject _characterObjectToRotate;
    public float RotateToFaceMovementDirectionSpeed = 10f;
    [Tooltip("the threshold after which we start rotating (absolute mode only)")]
    public float AbsoluteThresholdMovement = 0.5f;

    [Header("Animation Parameter"), Space()]
    [Tooltip("the speed at which the instant rotation animation parameter float resets to 0")]
    public float RotationSpeedResetSpeed = 2f;
    [Tooltip("the speed at which the YRotationOffsetSmoothed should lerp")]
    public float RotationOffsetSmoothSpeed = 1f;

    //Actual rotation Variable
    private Vector3 _currentDirection;
    private Quaternion _tmpRotation;
    private Quaternion _newMovementQuaternion;
    private Vector3 _lastMovement;

    //Animation Variable
    private Vector3 _newSpeed;
    private Vector3 _positionLastFrame;
    private Vector3 _relativeSpeed;
    private Vector3 _relativeMaximum;
    private Vector3 _remappedSpeed;
    private Vector3 _relativeSpeedNormalized;
    private float _yRotationOffset;
    private float _yRotationOffsetSmoothed;
    private float _modelAnglesYLastFrame;
    private float _rotationSpeed;


    [Tooltip("the direction of the model"), ReadOnly]
    public Vector3 ModelDirection;
    [Tooltip("the direction of the model in angle values"), ReadOnly]
    public Vector3 ModelAngles;

    protected override void StartInitialization()
    {
        base.StartInitialization();
        if (_characterObjectToRotate == null)
        {
            _characterObjectToRotate = _model;
        }
    }

    public override void ProcessBehaviour()
    {
        base.ProcessBehaviour();
        RotateToFaceMovementDirection();
        RotateModel();
    }

    private void FixedUpdate()
    {
        ComputeRelativeSpeeds();
    }

    private void RotateToFaceMovementDirection()
    {
        _currentDirection = _characterBase.CurrentDirection;
        // if the rotation mode is instant, we simply rotate to face our direction
        if (MovementRotationSpeed == RotationSpeeds.Instant)
        {
            if (_currentDirection != Vector3.zero)
            {
                _newMovementQuaternion = Quaternion.LookRotation(_currentDirection);
            }
        }

        // if the rotation mode is smooth, we lerp towards our direction
        if (MovementRotationSpeed == RotationSpeeds.Smooth)
        {
            if (_currentDirection != Vector3.zero)
            {
                _tmpRotation = Quaternion.LookRotation(_currentDirection);
                _newMovementQuaternion = Quaternion.Slerp(_characterObjectToRotate.transform.rotation, _tmpRotation, Time.deltaTime * RotateToFaceMovementDirectionSpeed);
            }
        }

        // if the rotation mode is smooth, we lerp towards our direction even if the input has been released
        if (MovementRotationSpeed == RotationSpeeds.SmoothAbsolute)
        {
            if (_currentDirection.normalized.magnitude >= AbsoluteThresholdMovement)
            {
                _lastMovement = _currentDirection;
            }
            if (_lastMovement != Vector3.zero)
            {
                _tmpRotation = Quaternion.LookRotation(_lastMovement);
                _newMovementQuaternion = Quaternion.Slerp(_characterObjectToRotate.transform.rotation, _tmpRotation, Time.deltaTime * RotateToFaceMovementDirectionSpeed);
            }
        }

        ModelDirection = _characterObjectToRotate.transform.forward.normalized;
        ModelAngles = _characterObjectToRotate.transform.eulerAngles;
    }

    private void RotateModel()
    {
        _characterObjectToRotate.transform.rotation = _newMovementQuaternion;

        //if (_shouldRotateTowardsWeapon && (_weaponRotationDirection != Vector3.zero))
        //{
        //    WeaponRotatingModel.transform.rotation = _newWeaponQuaternion;
        //}
    }

    /// <summary>
    /// Computes the relative speeds for Animations.
    /// </summary>
    protected virtual void ComputeRelativeSpeeds()
    {
        if (_characterObjectToRotate == null)
        {
            return;
        }

        if (Time.deltaTime != 0f)
        {
            _newSpeed = (this.transform.position - _positionLastFrame) / Time.deltaTime;
        }

        _relativeSpeed = _characterObjectToRotate.transform.InverseTransformVector(_newSpeed);


        float maxSpeed = 0f;
        if (_characterMovementBehaviour != null)
        {
            maxSpeed = _characterMovementBehaviour.WalkSpeed;
        }
        //if (_characterRun != null)
        //{
        //    maxSpeed = _characterRun.RunSpeed;
        //}

        _relativeMaximum = _characterBase.transform.TransformVector(Vector3.one);

        _remappedSpeed.x = Remap(_relativeSpeed.x, 0f, maxSpeed, 0f, _relativeMaximum.x);
        _remappedSpeed.y = Remap(_relativeSpeed.y, 0f, maxSpeed, 0f, _relativeMaximum.y);
        _remappedSpeed.z = Remap(_relativeSpeed.z, 0f, maxSpeed, 0f, _relativeMaximum.z);

        // relative speed normalized
        _relativeSpeedNormalized = _relativeSpeed.normalized;
        _yRotationOffset = _modelAnglesYLastFrame - ModelAngles.y;

        _yRotationOffsetSmoothed = Mathf.Lerp(_yRotationOffsetSmoothed, _yRotationOffset, RotationOffsetSmoothSpeed * Time.deltaTime);

        // RotationSpeed
        if (Mathf.Abs(_modelAnglesYLastFrame - ModelAngles.y) > 1f)
        {
            _rotationSpeed = Mathf.Abs(_modelAnglesYLastFrame - ModelAngles.y);
        }
        else
        {
            _rotationSpeed -= Time.time * RotationSpeedResetSpeed;
        }
        if (_rotationSpeed <= 0f)
        {
            _rotationSpeed = 0f;
        }

        _modelAnglesYLastFrame = ModelAngles.y;
        _positionLastFrame = this.transform.position;
    }

    public virtual void Face(CharacterBase.FacingDirections direction)
    {
        switch (direction)
        {
            case CharacterBase.FacingDirections.East:
                _newMovementQuaternion = Quaternion.LookRotation(Vector3.right);
                break;
            case CharacterBase.FacingDirections.North:
                _newMovementQuaternion = Quaternion.LookRotation(Vector3.forward);
                break;
            case CharacterBase.FacingDirections.South:
                _newMovementQuaternion = Quaternion.LookRotation(Vector3.back);
                break;
            case CharacterBase.FacingDirections.West:
                _newMovementQuaternion = Quaternion.LookRotation(Vector3.left);
                break;
        }
    }


    public float Remap(float x, float A, float B, float C, float D)
    {
        float remappedValue = C + (x - A) / (B - A) * (D - C);
        return remappedValue;
    }

    private void Reset()
    {
        RotateToFaceMovementDirectionSpeed = 10f;
        AbsoluteThresholdMovement = 0.5f;

        RotationSpeedResetSpeed = 2f;
        RotationOffsetSmoothSpeed = 1f;
    }
}
