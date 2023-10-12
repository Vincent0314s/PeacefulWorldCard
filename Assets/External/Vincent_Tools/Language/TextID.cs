using UnityEngine;

public abstract class TextID : MonoBehaviour,ILanguage
{
    public int ID;
    public bool isUsingDefaultFont;
  
    private void Start()
    {
        LanguageManager.i.AddTextIDToList(this);
        LanguageManager.i.SetLanguage(PlayerPrefs.GetInt("LangIndex"));
    }

    public abstract string GetText();
    public abstract void UpdateLanguage();
}
