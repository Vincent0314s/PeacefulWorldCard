using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CompareTextCharacterInFile : MonoBehaviour
{
    public TextAsset DialogueTest;
    public List<char> AllCharacters = new List<char>();

    public TMP_Animated tMPAnimated;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //ReadDialogueFromTextFile();
            tMPAnimated.ReadText("<emotion=angry><speed=30><b>DON'T COME CLOSE I'M NOT IN A GOOD MOOD!!!</b>");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            CompareCharacters();
        }
    }

    public void ReadDialogueFromTextFile()
    {
        string allText = DialogueTest.text;
        foreach (var cr in allText)
        {
            AllCharacters.Add(cr);
        }
    }

    public void CompareCharacters()
    {
        if (AllCharacters[0].Equals(AllCharacters[1]))
        {
            Debug.Log("Same");
        }
        else
        {
            Debug.Log("Different");
        }
    }
}
