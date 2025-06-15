using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour, IInitialization, IInitiazationEnable
{
    public static PlayerInputController Instance { get; private set; }
    private KeyCode _interactableKey = KeyCode.E;
    private KeyCode _inventoryKey = KeyCode.I;
    private KeyCode _keyboard_num1 = KeyCode.Alpha1;
    private KeyCode _keyboard_num2 = KeyCode.Alpha2;
    private KeyCode _keyboard_num3 = KeyCode.Alpha3;

    public bool PressInteractableKey => Input.GetKeyDown(_interactableKey);
    public bool HoldInteractableKey => Input.GetKey(_interactableKey);
    public bool PressInventoryKey => Input.GetKeyDown(_inventoryKey);
    public bool PressNum1Key => Input.GetKeyDown(_keyboard_num1);
    public bool PressNum2Key => Input.GetKeyDown(_keyboard_num2);
    public bool PressNum3Key => Input.GetKeyDown(_keyboard_num3);
    public Vector2 MoveInput { get; private set; }
    public Vector3 MousePosition { get; private set; }

    private PlayerInputActions playerInputActions;

    [Header("PlayerInput")]
    private InputAction movement;
    private InputAction rotateCamera;
    private InputAction zoomCamera;

    private InputAction mouseLeftClick;
    private InputAction mouseRightClick;

    private InputAction spaceKey;

    public void IAwake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        playerInputActions = new PlayerInputActions();
    }

    public void IStart()
    {
    }

    public void IEnable()
    {
        movement = playerInputActions.Player.Move;
        rotateCamera = playerInputActions.Player.RotateCamera;
        zoomCamera = playerInputActions.Player.ZoomCamera;

        spaceKey = playerInputActions.Player.SpaceKey;

        mouseLeftClick = playerInputActions.Player.MouseLeftClick;
        mouseRightClick = playerInputActions.Player.MouseRightClick;

        playerInputActions.Player.Enable();
    }

    public void IDisable()
    {
        playerInputActions.Player.Disable();
    }

    public Vector2 GetWASDMovementValue()
    {
        return movement.ReadValue<Vector2>();
    }

    public float GetRotateCameraValue()
    {
        return rotateCamera.ReadValue<Vector2>().x;
    }

    public float GetZoomCameraValue()
    {
        return zoomCamera.ReadValue<Vector2>().y;
    }


    public void ProcessInputBehaviour()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        MoveInput = new Vector2(horizontal, vertical);

        MousePosition = Input.mousePosition;
    }
}
