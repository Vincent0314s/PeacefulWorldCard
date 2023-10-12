using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LanguageManager : MonoBehaviour
{

    private static LanguageManager _i;
    public static LanguageManager i {
        get {
            if (_i == null) {
                _i = FindObjectOfType<LanguageManager>();
            }
            return _i;
        }
    }

    public TMP_FontAsset enFont { get; private set; }
    public TMP_FontAsset cnFont { get; private set; }
    private TextAsset txt;
    private string allText;
    private string[] splitAllText;
    public List<string> en = new List<string>();
    public List<string> ch = new List<string>();
    [SerializeField]
    private List<string> current = new List<string>();

    public List<ILanguage> textIDs = new List<ILanguage>();
    
    int langIndex;

    void Awake()
    {
        txt = Resources.Load<TextAsset>("Language");
        enFont = Resources.Load<TMP_FontAsset>("EnFont");
        cnFont = Resources.Load<TMP_FontAsset>("CnFont");
        allText = txt.text;
        splitAllText = allText.Split('=', '\n');

        for (int i = 0; i < splitAllText.Length; i += 2)
        {
            en.Add(splitAllText[i]);
        }
        for (int j = 1; j < splitAllText.Length; j += 2)
        {
            ch.Add(splitAllText[j]);
        }
        
        SetLanguage(PlayerPrefs.GetInt("LangIndex"));
    }

    public void SetLanguage(int index) {
        switch (index) {
            case 0://English
                current = en;
                break;
            case 1://Chinese
                current = ch;
                break;
        }
        langIndex = index;
        PlayerPrefs.SetInt("LangIndex",langIndex);
        for (int i = 0; i < textIDs.Count; i++)
        {
            textIDs[i].UpdateLanguage();
        }
    }

    public string GetLangText(int i)
    {
        return current[i];
    }

    public void AddTextIDToList(ILanguage text) {
        textIDs.Add(text);
    }

    public string[] GetDialogueText(int startIndex, int endIndex)
    {
        int lengh = endIndex - startIndex;
        string[] Temp = new string[lengh];
        current.CopyTo(startIndex,Temp,0, lengh);
        return Temp;
    }
}
