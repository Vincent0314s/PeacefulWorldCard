using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] PlayerInputController m_playerInput;
    private Transform m_cameraTransform;
    private Camera m_camera;
    [Header("Movement")]
    public float movementSpeed = 0.1f;
    public float movementTime = 5f;

    [Header("Rotation"), Space()]
    public float rotationSpeed = 1f;

    [Header("Zoom"),Space()]
    public Vector2 heightRange = new Vector2(6.5f,20);
    public float zoomDivider = 100;
    public float zoomSpeed = 1f;

    private Vector3 newPosition;
    private Quaternion newRotation;
    private Vector3 newZoom;

    private void Awake()
    {
        m_cameraTransform = transform.GetChild(0);
        m_camera = m_cameraTransform.GetComponent<Camera>();
    }

    private void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = m_cameraTransform.localPosition;
    }

    void Update()
    {
        UpdatePosition();
        UpdateRotation();
        UpdateZoomDistance();
    }

    private void UpdatePosition() {
        Vector3 inputValue = m_playerInput.GetWASDMovementValue().x * transform.right
                               + m_playerInput.GetWASDMovementValue().y * transform.forward;

        //inputValue = inputValue.normalized;
        newPosition += inputValue * movementSpeed;

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
    }

    private void UpdateRotation() {

        newRotation *= Quaternion.Euler(Vector3.up * m_playerInput.GetRotateCameraValue() * rotationSpeed);

        transform.rotation = Quaternion.Lerp(transform.rotation,newRotation,Time.deltaTime*movementTime);
    }

    private void UpdateZoomDistance()
    {
        float zoomHeight = (-m_playerInput.GetZoomCameraValue() / zoomDivider);

        newZoom += Vector3.up * zoomHeight * zoomSpeed;
        float posY = newZoom.y;
        posY = Mathf.Clamp(posY, heightRange.x, heightRange.y);
        newZoom = new Vector3(0,posY,0);

        m_cameraTransform.localPosition = Vector3.Lerp(m_cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }
}
