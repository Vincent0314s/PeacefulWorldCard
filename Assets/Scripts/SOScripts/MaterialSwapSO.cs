using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MaterialSwapFile", menuName = "Utility/MaterialSwapFile")]
public class MaterialSwapSO : ScriptableObject
{
    [System.Serializable]
    public struct MaterialType
    {
        public string MaterialName;
        public Material MaterialInstance;
    }

    [SerializeField] private MaterialType[] _materials;

    public void SwapMaterial(MeshRenderer meshRen, string materialName)
    {
        for (int i = 0; i < _materials.Length; i++)
        {
            if (_materials[i].MaterialName.Equals(materialName))
            {
                meshRen.material = _materials[i].MaterialInstance;
            }
        }
    }
}
