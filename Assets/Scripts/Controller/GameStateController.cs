using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameStateController : MonoBehaviour
{
    public enum GmaeState
    {
        None,
        PaperRockScissors,
        Start,
        End
    }

    public enum TurnOrder
    {
        None,
        Player,
        Enemy
    }

    public enum PaperRockScissors
    {
        Paper,
        Rock,
        Scissors
    }

    public GmaeState gmaeState;
    public TurnOrder turnOrder;
    public PaperRockScissors PlayerPeperRockScissors;
    public PaperRockScissors EnemyPeperRockScissors;

    [SerializeField] private Sprite[] _psr;
    [SerializeField] private GameObject _playerPSRObject;
    [SerializeField] private GameObject _enemyPSRObject;
    [SerializeField] private Image _playerPSR;
    [SerializeField] private Image _enemyPSR;

    [SerializeField] private TextMeshProUGUI _turnOrderText;

    private void Start()
    {
        gmaeState = GmaeState.None;
        turnOrder = TurnOrder.None;
    }

    public void GameStart()
    {
        gmaeState = GmaeState.PaperRockScissors;
        PaperRockScissorsSequence();
    }

    private void PaperRockScissorsSequence()
    {
        StartCoroutine(PaperRockScissorsCoroutine());
    }

    private IEnumerator PaperRockScissorsCoroutine()
    {
        _playerPSRObject.SetActive(true);
        _enemyPSRObject.SetActive(true);

        _playerPSR.sprite = GetRandomPaperRockScissorsResult();
        _enemyPSR.sprite = GetRandomPaperRockScissorsResult();
        yield return new WaitForSeconds(0.25f);
        _playerPSR.sprite = GetRandomPaperRockScissorsResult();
        _enemyPSR.sprite = GetRandomPaperRockScissorsResult();
        yield return new WaitForSeconds(0.25f);
        _playerPSR.sprite = GetRandomPaperRockScissorsResult();
        _enemyPSR.sprite = GetRandomPaperRockScissorsResult();
        yield return new WaitForSeconds(0.25f);
        _playerPSR.sprite = GetRandomPaperRockScissorsResult();
        _enemyPSR.sprite = GetRandomPaperRockScissorsResult();
        yield return new WaitForSeconds(0.25f);
        _playerPSR.sprite = GetRandomPaperRockScissorsResult();
        _enemyPSR.sprite = GetRandomPaperRockScissorsResult();
        yield return new WaitForSeconds(1f);

        CheckWhoWinsPeperRockScissors();
    }

    private void CheckWhoWinsPeperRockScissors()
    {
        Sprite playerSprite = _playerPSR.sprite;
        Sprite enemySprite = _enemyPSR.sprite;

        if (playerSprite.name.Contains("Paper"))
        {
            PlayerPeperRockScissors = PaperRockScissors.Paper;
        }
        else if (playerSprite.name.Contains("Rock"))
        {
            PlayerPeperRockScissors = PaperRockScissors.Rock;
        }
        else if (playerSprite.name.Contains("Scissors"))
        {
            PlayerPeperRockScissors = PaperRockScissors.Scissors;
        }

        if (enemySprite.name.Contains("Paper"))
        {
            EnemyPeperRockScissors = PaperRockScissors.Paper;
        }
        else if (enemySprite.name.Contains("Rock"))
        {
            EnemyPeperRockScissors = PaperRockScissors.Rock;
        }
        else if (enemySprite.name.Contains("Scissors"))
        {
            EnemyPeperRockScissors = PaperRockScissors.Scissors;
        }

        switch (PlayerPeperRockScissors)
        {
            case PaperRockScissors.Paper:
                switch (EnemyPeperRockScissors)
                {
                    case PaperRockScissors.Paper:
                        PaperRockScissorsSequence();
                        break;
                    case PaperRockScissors.Rock:
                        turnOrder = TurnOrder.Player;
                        break;
                    case PaperRockScissors.Scissors:
                        turnOrder = TurnOrder.Enemy;
                        break;
                }
                break;
            case PaperRockScissors.Rock:
                switch (EnemyPeperRockScissors)
                {
                    case PaperRockScissors.Paper:
                        turnOrder = TurnOrder.Enemy;
                        break;
                    case PaperRockScissors.Rock:
                        PaperRockScissorsSequence();
                        break;
                    case PaperRockScissors.Scissors:
                        turnOrder = TurnOrder.Player;
                        break;
                }
                break;
            case PaperRockScissors.Scissors:
                switch (EnemyPeperRockScissors)
                {
                    case PaperRockScissors.Paper:
                        turnOrder = TurnOrder.Player;
                        break;
                    case PaperRockScissors.Rock:
                        turnOrder = TurnOrder.Enemy;
                        break;
                    case PaperRockScissors.Scissors:
                        PaperRockScissorsSequence();
                        break;
                }
                break;
        }

        if (turnOrder != TurnOrder.None)
        {
            _turnOrderText.text = turnOrder.ToString();
            _playerPSRObject.SetActive(false);
            _enemyPSRObject.SetActive(false);
            gmaeState = GmaeState.Start;
        }
    }

    public void SwapTurnOrder()
    {
        if (turnOrder == TurnOrder.Player)
        {
            turnOrder = TurnOrder.Enemy;
        }
        else if (turnOrder == TurnOrder.Enemy)
        {
            turnOrder = TurnOrder.Player;
        }
        _turnOrderText.text = turnOrder.ToString();
    }

    private Sprite GetRandomPaperRockScissorsResult()
    {
        Sprite sprite = _psr[Random.Range(0, _psr.Length)];
        return sprite;
    }
}
