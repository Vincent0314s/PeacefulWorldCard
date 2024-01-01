using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GamePlayUIController : MonoBehaviour, IInitialization
{
    [SerializeField] private GameObject _playerPSRObject;
    [SerializeField] private GameObject _enemyPSRObject;
    //Paper = 0, Scissor = 1, Rock = 2
    public CustomButton[] PaperScissorRockButtons;

    public Image EnemyPSRImage { get; private set; }

    [SerializeField] private TextMeshProUGUI _turnOrderText;

    public void IAwake()
    {
        EnemyPSRImage = _enemyPSRObject.transform.GetChild(1).GetComponent<Image>();
    }

    public void IStart()
    {
    }

    public void SetEnemyPaperScissorsRockSprite(Sprite newSprite)
    {
        EnemyPSRImage.sprite = newSprite;
    }

    public void SetPaperScissorRockActive(bool isEnabled)
    {
        _playerPSRObject.SetActive(isEnabled);
        _enemyPSRObject.SetActive(isEnabled);
    }

    public void SetTurnOrderText(string newText)
    {
        _turnOrderText.text = newText;
    }


}
