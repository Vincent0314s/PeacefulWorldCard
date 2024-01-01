using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameBoardStateController : MonoBehaviour
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

    [SerializeField] private GamePlayUIController _gamePlayUIController;
    [SerializeField] private SpriteListSO _paperScissorRockSpriteList;

    private Sprite playerSprite;
    private Sprite enemySprite;

    private void Start()
    {
        gmaeState = GmaeState.None;
        turnOrder = TurnOrder.None;

        playerSprite = _gamePlayUIController.PlayerPSRImage.sprite;
        enemySprite = _gamePlayUIController.EnemyPSRImage.sprite;
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
        _gamePlayUIController.SetPaperScissorRockActive(true);

        yield return new WaitForSeconds(0.25f);
        AssignPaperScissorRockSprite();
        yield return new WaitForSeconds(0.25f);
        AssignPaperScissorRockSprite();
        yield return new WaitForSeconds(0.25f);
        AssignPaperScissorRockSprite();
        yield return new WaitForSeconds(0.25f);
        AssignPaperScissorRockSprite();
        yield return new WaitForSeconds(1f);

        CheckWhoWinsPeperRockScissors();
    }

    private void AssignPaperScissorRockSprite()
    {
        playerSprite = _paperScissorRockSpriteList.GetRandomSpriteFromList();
        enemySprite = _paperScissorRockSpriteList.GetRandomSpriteFromList();
    }

    private void CheckWhoWinsPeperRockScissors()
    {
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
            _gamePlayUIController.SetTurnOrderText(turnOrder.ToString());
            _gamePlayUIController.SetPaperScissorRockActive(false);
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
        _gamePlayUIController.SetTurnOrderText(turnOrder.ToString());
    }
}
