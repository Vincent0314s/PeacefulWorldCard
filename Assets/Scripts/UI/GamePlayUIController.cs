using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GamePlayUIController : MonoBehaviour
{
    [SerializeField] private GameObject _playerPSRObject;
    [SerializeField] private GameObject _enemyPSRObject;
    public Image PlayerPSRImage { get; private set; }
    public Image EnemyPSRImage { get; private set; }

    [SerializeField] private TextMeshProUGUI _turnOrderText;

    private void Awake()
    {
        PlayerPSRImage = _playerPSRObject.transform.GetChild(1).GetComponent<Image>();
        EnemyPSRImage = _enemyPSRObject.transform.GetChild(1).GetComponent<Image>();
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
