using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public enum FacingDirections { West, East, North, South }
    public enum CharacterType { Player, AI }

    private CharacterController _characterController;
    private CharacterBehaviour[] _characterBehaviours;
    private PlayerInputController _inputController;

    //Velocity
    private Vector3 _newVelocity;
    private Vector3 _idealVelocity;
    private Vector3 _motion;
    private Vector3 _horizontalVelocityDelta;

    //Position
    private Vector3 _positionLastFrame;

    private Vector3 _lastHitPoint;
    private Vector3 _hitPoint;
    private Vector3 _lastGroundNormal;
    private float _stickOffset;
    protected Vector3 _impact;

    public CharacterType Type;

    [Header("Baseic Values"), Space()]
    public float Gravity;
    public float MaximumFallSpeed;
    public float ImpactFalloff = 5f;

    [Header("Reference"), Space()]
    public GameObject CharacterModel;
    public Animator CharacterAnimator;

    [Header("Debug"), ReadOnly, Space()]
    public Vector3 InputMoveDirection;
    [ReadOnly]
    public Vector3 Speed;
    [ReadOnly]
    public Vector3 Velocity;
    [ReadOnly]
    public Vector3 VelocityLastFrame;
    [ReadOnly]
    public Vector3 AccelerationVector;
    [ReadOnly]
    public bool IsGrounded;
    [ReadOnly]
    public Vector3 CurrentMovement;
    [ReadOnly]
    public Vector3 CurrentDirection;
    [ReadOnly]
    public Vector3 AddedForce;
    [ReadOnly]
    public Vector3 GroundNormal;



    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _characterBehaviours = GetComponents<CharacterBehaviour>();
        _inputController = GetComponent<PlayerInputController>();
    }

    private void Start()
    {
        CurrentDirection = transform.forward;
    }


    private void Update()
    {
        ProcessInputBehaviours();

        _newVelocity = Velocity;
        _positionLastFrame = transform.position;

        DetermineDirection();

        ProcessEarlyBehaviours();
        ProcessBehaviours();
        ProcessLateBehaviours();

        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        ApplyImpact();

        AddInput();
        AddGravity();
        ComputeVelocityDelta();
        MoveCharacterController();
        ComputeNewVelocity();
        HandleGroundContact();
        ComputeSpeed();
    }

    private void LateUpdate()
    {
        VelocityLastFrame = Velocity;
    }

    private void ProcessInputBehaviours()
    {
        _inputController.ProcessInputBehaviour();
        foreach (var behaviour in _characterBehaviours)
        {
            if (behaviour.enabled)
            {
                behaviour.ProcessInputBehaviour();
            }
        }
    }

    private void ProcessEarlyBehaviours()
    {
        foreach (var behaviour in _characterBehaviours)
        {
            if (behaviour.enabled)
            {
                behaviour.EarlyProcessBehaviour();
            }
        }
    }

    private void ProcessBehaviours()
    {
        foreach (var behaviour in _characterBehaviours)
        {
            if (behaviour.enabled)
            {
                behaviour.ProcessBehaviour();
            }
        }
    }

    private void ProcessLateBehaviours()
    {
        foreach (var behaviour in _characterBehaviours)
        {
            if (behaviour.enabled)
            {
                behaviour.LateProcessBehaviour();
            }
        }
    }

    private void UpdateAnimator()
    {
        foreach (var behaviour in _characterBehaviours)
        {
            if (behaviour.enabled)
            {
                behaviour.UpdateAnimator();
            }
        }
    }

    protected void DetermineDirection()
    {
        if (CurrentMovement.magnitude > 0f)
        {
            CurrentDirection = CurrentMovement.normalized;
        }
    }

    private void AddInput()
    {
        _idealVelocity = CurrentMovement;

        _idealVelocity.y = 0;

        if (IsGrounded)
        {
            Vector3 sideWays = Vector3.Cross(Vector3.up, _idealVelocity);
            _idealVelocity = Vector3.Cross(sideWays, GroundNormal).normalized * _idealVelocity.magnitude;
        }

        _newVelocity = _idealVelocity;
        _newVelocity.y = IsGrounded ? Mathf.Min(_newVelocity.y, 0) : _newVelocity.y;
    }

    private void AddGravity()
    {
        if (IsGrounded)
        {
            _newVelocity.y = Mathf.Min(0, _newVelocity.y) - Gravity * Time.deltaTime;
        }
        else
        {
            _newVelocity.y = Velocity.y - Gravity * Time.deltaTime;
            _newVelocity.y = Mathf.Max(_newVelocity.y, -MaximumFallSpeed);
        }

        _newVelocity += AddedForce;
        AddedForce = Vector3.zero;
    }

    private void ComputeVelocityDelta()
    {
        _motion = _newVelocity * Time.deltaTime;
        _horizontalVelocityDelta.x = _motion.x;
        _horizontalVelocityDelta.y = 0;
        _horizontalVelocityDelta.z = _motion.z;
        _stickOffset = Mathf.Max(_characterController.stepOffset, _horizontalVelocityDelta.magnitude);

        if (IsGrounded)
        {
            _motion -= _stickOffset * Vector3.up;
        }
    }

    private void MoveCharacterController()
    {
        GroundNormal.x = GroundNormal.y = GroundNormal.z = 0f;

        _characterController.Move(_motion);

        _lastHitPoint = _hitPoint;
        _lastGroundNormal = GroundNormal;
    }

    private void ComputeNewVelocity()
    {
        Velocity = _newVelocity;
        AccelerationVector = (Velocity - VelocityLastFrame) / Time.deltaTime;
    }

    protected virtual void HandleGroundContact()
    {
        IsGrounded = _characterController.isGrounded;

        if (IsGrounded && !IsGroundedTest())
        {
            IsGrounded = false;
        }
        else if (!IsGrounded && IsGroundedTest())
        {
            IsGrounded = Velocity.y <= 0;
        }

        //ExitedTooSteepSlopeThisFrame = (_tooSteepLastFrame && !TooSteep());
        //_tooSteepLastFrame = TooSteep();
    }

    public virtual bool IsGroundedTest()
    {
        bool grounded = true;
        GroundNormal.x = 0;
        GroundNormal.y = 1;
        GroundNormal.z = 0;

        return grounded;
    }

    protected virtual void ComputeSpeed()
    {
        if (Time.deltaTime != 0f)
        {
            Speed = (this.transform.position - _positionLastFrame) / Time.deltaTime;
        }
        // we round the speed to 2 decimals
        Speed.x = Mathf.Round(Speed.x * 100f) / 100f;
        Speed.y = Mathf.Round(Speed.y * 100f) / 100f;
        Speed.z = Mathf.Round(Speed.z * 100f) / 100f;
        _positionLastFrame = this.transform.position;
    }

    public void SetMovementVector(Vector3 movement)
    {
        CurrentMovement = movement;

        Vector3 directionVector;
        directionVector = movement;
        if (directionVector != Vector3.zero)
        {
            float directionLength = directionVector.magnitude;
            directionVector = directionVector / directionLength;
            directionLength = Mathf.Min(1, directionLength);
            directionLength = directionLength * directionLength;
            directionVector = directionVector * directionLength;
        }
        InputMoveDirection = transform.rotation * directionVector;
    }

    protected virtual void ApplyImpact()
    {
        if (_impact.magnitude > 0.2f)
        {
            _characterController.Move(_impact * Time.deltaTime);
        }
        _impact = Vector3.Lerp(_impact, Vector3.zero, ImpactFalloff * Time.deltaTime);
    }

    public virtual void Impact(Vector3 direction, float force)
    {
        direction = direction.normalized;
        if (direction.y < 0) { direction.y = -direction.y; }
        _impact += direction.normalized * force;
    }

    private void Reset()
    {
        Gravity = 40f;
        MaximumFallSpeed = 40f;
    }
}
