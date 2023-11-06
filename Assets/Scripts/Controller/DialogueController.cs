using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class DialogueController : MonoBehaviour
{
    public TextAsset[] DialogueTest;

    public TextMeshProUGUI CharacterNameText;
    public TMP_Animated DialogueContext;
    public Image BGImage;
    public Image CharacterImage;
    [SerializeField] private SpriteListSO _backgroundSO;
    [SerializeField] private SpriteListSO _characterSO;
    public List<string> AllDialogues = new List<string>();
    public List<string> CurrentDialogueList = new List<string>();

    public int TotalDialogueIndex;
    public int CurrentDialogueIndex;

    private void Start()
    {
        ReadDialogueFromTextFile(0);
        ReadDialogueByIndex();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!DialogueContext.IsLineFinished)
            {
                ReadDialogueByLine();
            }
            else
            {
                ReadDialogueByIndex();
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (CurrentDialogueIndex > 0)
            {
                CurrentDialogueIndex--;
                DialogueContext.ReadText(CurrentDialogueList[CurrentDialogueIndex]);
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (CurrentDialogueIndex < CurrentDialogueList.Count - 1)
            {
                CurrentDialogueIndex++;
                DialogueContext.ReadText(CurrentDialogueList[CurrentDialogueIndex]);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ReadDialogueFromTextFile(0);
            ReadDialogueByIndex();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ReadDialogueFromTextFile(1);
            ReadDialogueByIndex();
        }
    }

    public void ReadDialogueFromTextFile(int dialogueIndex)
    {
        string allText = DialogueTest[dialogueIndex].text;
        string[] textByline = allText.Split(System.Environment.NewLine.ToCharArray());
        AllDialogues.Clear();
        foreach (var line in textByline)
        {
            if (!string.IsNullOrEmpty(line))
            {
                AllDialogues.Add(line);
            }
        }
        TotalDialogueIndex = 0;
        CurrentDialogueIndex = 0;
        CurrentDialogueList.Clear();
    }


    public void ReadDialogueByIndex()
    {
        if (TotalDialogueIndex > 0 && TotalDialogueIndex < AllDialogues.Count - 1)
        {
            TotalDialogueIndex++;
        }

        for (int i = TotalDialogueIndex; i < AllDialogues.Count; i++)
        {
            string line = AllDialogues[i];
            if (line.Contains("[BACKGROUND"))
            {
                string backgroundName = line.Substring(line.IndexOf('=') + 1, line.IndexOf(']') - (line.IndexOf('=') + 1));
                BGImage.sprite = _backgroundSO.GetSpriteByName(backgroundName);
            }
            else if (line.Contains("[NAME"))
            {
                string characterName = line.Substring(line.IndexOf('=') + 1, line.IndexOf(']') - (line.IndexOf('=') + 1));
                CharacterNameText.text = characterName;
            }
            else if (line.Contains("[CHAR"))
            {
                string charName = line.Substring(line.IndexOf('=') + 1, line.IndexOf(']') - (line.IndexOf('=') + 1));
                CharacterImage.sprite = _characterSO.GetSpriteByName(charName);
            }
            else
            {
                DialogueContext.ReadText(line);
                if (!CurrentDialogueList.Contains(line))
                {
                    CurrentDialogueList.Add(line);
                }
                CurrentDialogueIndex = CurrentDialogueList.Count - 1;
                TotalDialogueIndex = i;
                return;
            }
        }
    }

    public void ReadDialogueByLine()
    {
        string line = AllDialogues[TotalDialogueIndex];
        DialogueContext.RevealAll(line);
    }
}
