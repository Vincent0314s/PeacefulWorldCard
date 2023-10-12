using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    public enum Angle { 
        X,
        Y,
        Z,
    }
    [Header("Floating")]
    public float force = 1f;
    public float frequencey = 1f;

    private float originalPosY;

    [Header("Rotation")]
    public bool canRotate;
    public Angle rotateAngle = Angle.Y;
    public float rotateSpeed = 90f;

    private void Start()
    {
        originalPosY = transform.position.y;
    }


    void Update()
    {
        
        float y =  Mathf.Sin(Time.deltaTime * frequencey) * force;
        transform.position = new Vector3(transform.position.x, originalPosY + y, transform.position.z);

        if (canRotate) {
            switch (rotateAngle) {
                case Angle.X:
                    transform.Rotate(rotateSpeed * Time.deltaTime, 0, 0,Space.World);
                    break;
                case Angle.Y:
                    transform.Rotate(0, rotateSpeed * Time.deltaTime, 0,Space.World);
                    break;
                case Angle.Z:
                    transform.Rotate(0, 0, rotateSpeed * Time.deltaTime,Space.World);
                    break;
            }
        }
    }
}
