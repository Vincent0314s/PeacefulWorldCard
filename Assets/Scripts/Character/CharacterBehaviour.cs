using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    protected CharacterBase _characterBase;
    protected PlayerInputController _inputController;
    protected Animator _animator;
    protected CharacterMovementBehaviour _characterMovementBehaviour;

    protected GameObject _model;
    protected float _verticalInput;
    protected float _horizontalInput;
    protected Vector3 _mousePosition;

    protected virtual void Awake()
    {
        AwakeInitialization();
    }

    protected virtual void Start()
    {
        StartInitialization();
    }

    protected virtual void AwakeInitialization()
    {
        _characterBase = GetComponent<CharacterBase>();
        _inputController = GetComponent<PlayerInputController>();
        _characterMovementBehaviour = GetComponent<CharacterMovementBehaviour>();
        BindAnimator();
    }

    protected virtual void StartInitialization()
    {
        _model = _characterBase.CharacterModel;
    }

    /// <summary>
    /// Binds the animator from the character and initializes the animator parameters
    /// </summary>
    protected virtual void BindAnimator()
    {
        _animator = _characterBase.CharacterAnimator;

        if (_animator != null)
        {
            InitializeAnimatorParameters();
        }
    }

    /// <summary>
    /// Adds required animator parameters to the animator parameters list if they exist
    /// </summary>
    protected virtual void InitializeAnimatorParameters()
    {

    }

    /// <summary>
    /// Called at the very start of the ability's cycle, and intended to be overridden, looks for input and calls methods if conditions are met
    /// </summary>
    protected virtual void HandleInput()
    {

    }

    /// <summary>
    /// Resets all input for this ability. Can be overridden for ability specific directives
    /// </summary>
    public virtual void ResetInput()
    {
        _horizontalInput = 0f;
        _verticalInput = 0f;
    }
    /// <summary>
    /// Internal method to check if an input manager is present or not
    /// </summary>
    public virtual void ProcessInputBehaviour()
    {
        _horizontalInput = _inputController.MoveInput.x;
        _verticalInput = _inputController.MoveInput.y;
        _mousePosition = _inputController.MousePosition;
        HandleInput();
    }

    /// <summary>
    /// The first of the 3 passes you can have in your ability. Think of it as EarlyUpdate() if it existed
    /// </summary>
    public virtual void EarlyProcessBehaviour()
    {

    }

    /// <summary>
    /// The second of the 3 passes you can have in your ability. Think of it as Update()
    /// </summary>
    public virtual void ProcessBehaviour()
    {

    }

    /// <summary>
    /// The last of the 3 passes you can have in your ability. Think of it as LateUpdate()
    /// </summary>
    public virtual void LateProcessBehaviour()
    {

    }

    /// <summary>
    /// Override this to send parameters to the character's animator. This is called once per cycle, by the Character class, after Early, normal and Late process().
    /// </summary>
    public virtual void UpdateAnimator()
    {

    }

    /// <summary>
    /// Changes the status of the ability's permission
    /// </summary>
    /// <param name="abilityPermitted">If set to <c>true</c> ability permitted.</param>
    public virtual void PermitBehaviour(bool abilityPermitted)
    {
        //AbilityPermitted = abilityPermitted;
    }

    /// <summary>
    /// Override this to reset this ability's parameters. It'll be automatically called when the character gets killed, in anticipation for its respawn.
    /// </summary>
    public virtual void ResetBehaviour()
    {

    }
}
