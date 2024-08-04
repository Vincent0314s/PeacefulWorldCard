using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
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

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void ProcessInputBehaviour()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        MoveInput = new Vector2(horizontal, vertical);

        MousePosition = Input.mousePosition;
    }
}
