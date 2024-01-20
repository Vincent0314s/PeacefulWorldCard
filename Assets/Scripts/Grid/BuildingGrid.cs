using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Card
{
    public class BuildingGrid : MonoBehaviour
    {
        [SerializeField] private MaterialSwapSO _materialSwap;
        private MeshRenderer _meshRen;
        private MeshCollider _meshCollider;
        public bool isSelected { get; private set; }
        public bool hasCardObject { get; private set; }

        private void Awake()
        {
            _meshRen = GetComponent<MeshRenderer>();
            _meshCollider = GetComponent<MeshCollider>();
        }

        public void Selected()
        {
            isSelected = true;
            _materialSwap.SwapMaterial(_meshRen, StringDefs.MaterialSwap.Selected);
        }

        public void DeSelected()
        {
            isSelected = false;
            _materialSwap.SwapMaterial(_meshRen, StringDefs.MaterialSwap.Normal);
        }

        public void HasCard()
        {
            hasCardObject = true;
            _materialSwap.SwapMaterial(_meshRen, StringDefs.MaterialSwap.Occupied);
            _meshCollider.enabled = false;
        }

        public void DisplayGrid(bool b)
        {
            _meshRen.enabled = b;
            _meshCollider.enabled = b;
        }

        public Vector3 GetTileCenterPosition()
        {
            return new Vector3(transform.position.x,0.2f,transform.position.z);
        }
    }
}

