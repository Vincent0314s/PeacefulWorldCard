using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class DialogueController : MonoBehaviour
{
    public TextAsset DialogueTest;

    public TextMeshProUGUI CharacterNameText;
    public TextMeshProUGUI DialogueContext;
    public Image BGImage;
    [SerializeField] private SpriteListSO backgroundSO;
    public List<string> AllDialogues = new List<string>();

    public int DialogueIndex;


    private void Start()
    {
        ReadDialogueFromTextFile();
        ReadDialogueByIndex();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ReadDialogueByIndex();
        }
    }

    public void ReadDialogueFromTextFile()
    {
        string allText = DialogueTest.text;
        string[] textByline = allText.Split(System.Environment.NewLine.ToCharArray());
        foreach (var line in textByline)
        {
            if (!string.IsNullOrEmpty(line))
            {
                AllDialogues.Add(line);
            }
        }
    }

    public void ReadDialogueByIndex()
    {
        if (DialogueIndex > 0 && DialogueIndex < AllDialogues.Count - 1)
        {
            DialogueIndex++;
        }
        for (int i = DialogueIndex; i < AllDialogues.Count; i++)
        {
            string line = AllDialogues[i];
            if (line.Contains("[BACKGROUND"))
            {
                string backgroundName = line.Substring(line.IndexOf('=') + 1, line.IndexOf(']') - (line.IndexOf('=') + 1));
                BGImage.sprite = backgroundSO.GetSpriteByName(backgroundName);
            }
            else if (line.Contains("[NAME"))
            {
                string characterName = line.Substring(line.IndexOf('=') + 1, line.IndexOf(']') - (line.IndexOf('=') + 1));
                CharacterNameText.text = characterName;
            }
            else if (line.Contains("[CHAR"))
            {
                Debug.Log("Assign Character Sprite");
            }
            else
            {
                DialogueContext.text = line;
                DialogueIndex = i;
                return;
            }
        }
    }
}
