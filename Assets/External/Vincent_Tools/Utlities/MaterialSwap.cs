using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MaterialSwapFile", menuName = "SO/MaterialSwapFile")]
public class MaterialSwap : ScriptableObject
{
    [SerializeField] private Material[] _materials;

    public void SwapMaterial(MeshRenderer meshRen, int materialIndex)
    {
        meshRen.material = _materials[materialIndex];
    }

}
