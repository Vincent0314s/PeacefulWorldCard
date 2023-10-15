using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SpriteList", menuName = "Utility/SpriteList", order = 2)]
public class SpriteListSO : ScriptableObject
{
    public Sprite[] Sprites;
    public string folderPath;


    public Sprite GetSpriteByName(string name)
    {
        for (int i = 0; i < Sprites.Length; i++)
        {
            if (Sprites[i].name.Equals(name)) { 
                return Sprites[i];
            }
        }
        return null;
    }
}
