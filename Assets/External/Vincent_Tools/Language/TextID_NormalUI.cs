using UnityEngine;
using UnityEngine.UI;

public class TextID_NormalUI : TextID
{
    public override void UpdateLanguage()
    {
        if (!isUsingDefaultFont) {
            if (PlayerPrefs.GetInt("LangIndex") == 0)
            {
                GetComponent<Text>().font = LanguageManager.i.enFont.sourceFontFile;
            }
            else
            {
                GetComponent<Text>().font = LanguageManager.i.cnFont.sourceFontFile;
            }
        }
       
        GetComponent<Text>().text = LanguageManager.i.GetLangText(ID);
    }

    public override string GetText()
    {
        return LanguageManager.i.GetLangText(ID);
    }
}
