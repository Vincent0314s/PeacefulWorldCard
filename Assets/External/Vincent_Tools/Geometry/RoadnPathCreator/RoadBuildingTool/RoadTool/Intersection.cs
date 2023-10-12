using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EditorTool {
    public class Intersection : MonoBehaviour
    {
        public Vector3[] linkPoints = new Vector3[4];
        public float intersectinoSize = 5f;
        private Transform intersectionTrans;

        public void UpdateSize()
        {
            if (intersectionTrans == null)
                intersectionTrans = transform.GetChild(0);

            intersectionTrans.transform.localScale = Vector3.one * intersectinoSize;
            float sizeX = intersectionTrans.transform.localScale.x;
            float sizeY = intersectionTrans.transform.localScale.y;
            //Up
            linkPoints[0] = new Vector3(transform.position.x, 0, sizeY / 2 + transform.position.z);
            //Down
            linkPoints[1] = new Vector3(transform.position.x, 0, -sizeY / 2 + transform.position.z);
            //Left
            linkPoints[2] = new Vector3(-sizeX / 2 + transform.position.x, 0, transform.position.z);
            //Right
            linkPoints[3] = new Vector3(sizeX / 2 + transform.position.x, 0, transform.position.z);
        }
    }
}

