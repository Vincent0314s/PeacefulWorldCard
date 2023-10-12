using UnityEngine;
using TMPro;

public class TextID_TextMeshUI : TextID
{

    public override void UpdateLanguage() {
        if (!isUsingDefaultFont) {
            if (PlayerPrefs.GetInt("LangIndex") == 0)
            {
                GetComponent<TextMeshProUGUI>().font = LanguageManager.i.enFont;
            }
            else
            {
                GetComponent<TextMeshProUGUI>().font = LanguageManager.i.cnFont;
            }
            GetComponent<TextMeshProUGUI>().UpdateFontAsset();
        }
        GetComponent<TextMeshProUGUI>().text = LanguageManager.i.GetLangText(ID);
    }

    public override string GetText()
    {
        return LanguageManager.i.GetLangText(ID);
    }
}
