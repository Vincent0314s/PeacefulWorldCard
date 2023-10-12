using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowGizmos : MonoBehaviour
{
    
    [SerializeField]private Color color = Color.red;
    [SerializeField]private float radius = 0.25f;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position,radius);
    }

    public void SetGizmos(Color _color,float _radius) {
        color = _color;
        radius = _radius;
    }
}
