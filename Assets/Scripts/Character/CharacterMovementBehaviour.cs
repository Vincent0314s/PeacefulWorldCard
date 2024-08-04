using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementBehaviour : CharacterBehaviour
{
    public float WalkSpeed;
    public float Acceleration;
    public float Deceleration;
    public float IdleThreshold;
    public bool RotateInputBasedOnCameraDirection;

    //Movement Variable
    public float MovementSpeed { get; set; }
    protected Vector3 _movementVector;
    protected Vector2 _currentInput;

    //protected float _horizontalMovement;
    //protected float _verticalMovement;

    protected Vector2 _normalizedInput;
    protected Vector2 _lerpedInput;
    private float _acceleration;
    private float _movementSpeed;
    private float _movementSpeedMultiplier;
    public float MovementSpeedMaxMultiplier { get; set; } = float.MaxValue;

    public float MovementSpeedMultiplier
    {
        get => Mathf.Min(_movementSpeedMultiplier, MovementSpeedMaxMultiplier);
        set => _movementSpeedMultiplier = value;
    }

    //Camera Variable
    protected float _cameraAngle;

    protected override void StartInitialization()
    {
        base.StartInitialization();
        MovementSpeedMultiplier = 1f;
        MovementSpeed = WalkSpeed;
    }


    public override void ProcessBehaviour()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        _movementVector = Vector3.zero;
        _currentInput = Vector2.zero;

        _currentInput.x = _horizontalInput;
        _currentInput.y = _verticalInput;
        if (RotateInputBasedOnCameraDirection)
        {
            _currentInput = ApplyCameraRotation(_currentInput);
        }

        _normalizedInput = _currentInput.normalized;

        if ((Acceleration == 0) || (Deceleration == 0))
        {
            _lerpedInput = _normalizedInput;
        }
        else
        {
            if (_normalizedInput.magnitude == 0)
            {
                _acceleration = Mathf.Lerp(_acceleration, 0f, Deceleration * Time.deltaTime);
                _lerpedInput = Vector2.Lerp(_lerpedInput, _lerpedInput * _acceleration, Time.deltaTime * Deceleration);
            }
            else
            {
                _acceleration = Mathf.Lerp(_acceleration, 1f, Acceleration * Time.deltaTime);
                _lerpedInput = Vector2.ClampMagnitude(_normalizedInput, _acceleration);
            }
        }

        _movementVector.x = _lerpedInput.x;
        _movementVector.y = 0f;
        _movementVector.z = _lerpedInput.y;
        _movementSpeed = MovementSpeed * MovementSpeedMultiplier;
        _movementVector *= _movementSpeed;

        if (_movementVector.magnitude > MovementSpeed * MovementSpeedMultiplier)
        {
            _movementVector = Vector3.ClampMagnitude(_movementVector, MovementSpeed);
        }

        if ((_currentInput.magnitude <= IdleThreshold) && (_characterBase.CurrentMovement.magnitude < IdleThreshold))
        {
            _movementVector = Vector3.zero;
        }

        _characterBase.SetMovementVector(_movementVector);
    }

    public void MovePosition(Vector3 newPosition)
    {
        //_movePositionDirection = (newPosition - this.transform.position);
        //_movePositionDistance = Vector3.Distance(this.transform.position, newPosition);

        //_capsulePoint1 = this.transform.position
        //                    + _characterController.center
        //                    - (Vector3.up * _characterController.height / 2f)
        //                    + Vector3.up * _characterController.skinWidth
        //                    + Vector3.up * _characterController.radius;
        //_capsulePoint2 = this.transform.position
        //                    + _characterController.center
        //                    + (Vector3.up * _characterController.height / 2f)
        //                    - Vector3.up * _characterController.skinWidth
        //                    - Vector3.up * _characterController.radius;

        //if (!Physics.CapsuleCast(_capsulePoint1, _capsulePoint2, _characterController.radius, _movePositionDirection, out _movePositionHit, _movePositionDistance, ObstaclesLayerMask))
        //{
        //    this.transform.position = newPosition;
        //}
    }

    public override void UpdateAnimator()
    {
        _animator.SetFloat("MoveSpeed", _movementVector.magnitude);
    }

    public Vector2 ApplyCameraRotation(Vector2 input)
    {
        if (RotateInputBasedOnCameraDirection)
        {
            _cameraAngle = _characterBase.CharacterCamera.transform.localEulerAngles.y;
            return MathUtility.RotateVector2(input, -_cameraAngle);
        }
        else
        {
            return input;
        }
    }

    private void Reset()
    {
        WalkSpeed = 6f;
        Acceleration = 10f;
        Deceleration = 100f;
        IdleThreshold = 0.05f;
    }
}
