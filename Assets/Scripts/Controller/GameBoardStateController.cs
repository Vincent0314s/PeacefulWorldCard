using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum PaperRockScissors
{
    Paper,
    Rock,
    Scissors
}

public class GameBoardStateController : MonoBehaviour, IInitialization
{
    public enum GmaeState
    {
        None,
        PaperRockScissorsStart,
        GameStart,
        GameEnd
    }

    public enum TurnOrder
    {
        None,
        Player,
        Enemy
    }

    public GmaeState gmaeState;
    public TurnOrder turnOrder;
    public PaperRockScissors PlayerPaperRockScissors;
    public PaperRockScissors EnemyPaperRockScissors;

    [Space(), Header("Reference")]
    [SerializeField] private GamePlayUIController _gamePlayUIController;
    [SerializeField] private PaperScissorRockSequenceController _paperScissorRockSequenceController;
    [SerializeField] private SpriteListSO _paperScissorRockSpriteList;

    public void IAwake()
    {
    }

    public void IStart()
    {
        gmaeState = GmaeState.None;
        turnOrder = TurnOrder.None;

        _gamePlayUIController.PaperScissorRockButtons[0].AddButtonClickEvent(() => PlayerDonePaperScissorRockSelection(PaperRockScissors.Paper));
        _gamePlayUIController.PaperScissorRockButtons[1].AddButtonClickEvent(() => PlayerDonePaperScissorRockSelection(PaperRockScissors.Scissors));
        _gamePlayUIController.PaperScissorRockButtons[2].AddButtonClickEvent(() => PlayerDonePaperScissorRockSelection(PaperRockScissors.Rock));
        _gamePlayUIController.SetPaperScissorRockActive(false);
    }

    public void StartPaperScissorRock()
    {
        gmaeState = GmaeState.PaperRockScissorsStart;
        _gamePlayUIController.SetPaperScissorRockActive(true);
        ResetPaperScissorsRock();
    }
    private void PlayerDonePaperScissorRockSelection(PaperRockScissors paperRockScissors)
    {
        PlayerPaperRockScissors = paperRockScissors;

        StartCoroutine(PaperRockScissorsSequenceCoroutine());
    }

    private IEnumerator PaperRockScissorsSequenceCoroutine()
    {
        switch (PlayerPaperRockScissors)
        {
            case PaperRockScissors.Paper:
                _paperScissorRockSequenceController.PaperSelection();
                break;
            case PaperRockScissors.Scissors:
                _paperScissorRockSequenceController.ScissorsSelection();
                break;
            case PaperRockScissors.Rock:
                _paperScissorRockSequenceController.RockSelection();
                break;
        }
        yield return new WaitForSeconds(1f);
        _gamePlayUIController.SetEnemyPaperScissorsRockSprite(_paperScissorRockSpriteList.GetRandomSpriteFromList()); 
        yield return new WaitForSeconds(1f);
        CheckWhoWinsPeperRockScissors();
    }

    private void ResetPaperScissorsRock()
    {
        _gamePlayUIController.SetEnemyPaperScissorsRockSprite(null);
        _paperScissorRockSequenceController.ResetSequence();
    }

    private void CheckWhoWinsPeperRockScissors()
    {
        Sprite enemySprite = _gamePlayUIController.EnemyPSRImage.sprite;

        if (enemySprite.name.Contains("Paper"))
        {
            EnemyPaperRockScissors = PaperRockScissors.Paper;
        }
        else if (enemySprite.name.Contains("Rock"))
        {
            EnemyPaperRockScissors = PaperRockScissors.Rock;
        }
        else if (enemySprite.name.Contains("Scissors"))
        {
            EnemyPaperRockScissors = PaperRockScissors.Scissors;
        }

        switch (PlayerPaperRockScissors)
        {
            case PaperRockScissors.Paper:
                switch (EnemyPaperRockScissors)
                {
                    case PaperRockScissors.Paper:
                        ResetPaperScissorsRock();
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
                switch (EnemyPaperRockScissors)
                {
                    case PaperRockScissors.Paper:
                        turnOrder = TurnOrder.Enemy;
                        break;
                    case PaperRockScissors.Rock:
                        ResetPaperScissorsRock();
                        break;
                    case PaperRockScissors.Scissors:
                        turnOrder = TurnOrder.Player;
                        break;
                }
                break;
            case PaperRockScissors.Scissors:
                switch (EnemyPaperRockScissors)
                {
                    case PaperRockScissors.Paper:
                        turnOrder = TurnOrder.Player;
                        break;
                    case PaperRockScissors.Rock:
                        turnOrder = TurnOrder.Enemy;
                        break;
                    case PaperRockScissors.Scissors:
                        ResetPaperScissorsRock();
                        break;
                }
                break;
        }

        if (turnOrder != TurnOrder.None)
        {
            _gamePlayUIController.SetTurnOrderText(turnOrder.ToString());
            _gamePlayUIController.SetPaperScissorRockActive(false);
            gmaeState = GmaeState.GameStart;
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
        _gamePlayUIController.SetTurnOrderText(turnOrder.ToString());
    }

    private void OnDisable()
    {
        for (int i = 0; i < _gamePlayUIController.PaperScissorRockButtons.Length; i++)
        {
            _gamePlayUIController.PaperScissorRockButtons[i].ClearButtonEvent();
        }
    }
}
