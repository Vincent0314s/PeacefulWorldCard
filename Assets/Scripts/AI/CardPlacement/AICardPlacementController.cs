using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Card;
using System;

public class AICardPlacementController : BasicCardPlacementController, IInitialization
{
    [Header("Shield")]
    [SerializeField] private BuildingGrid[] _enemyBuildingGrid;
    private List<BuildingGrid> _availableEnemyBuildingGrid;

    [SerializeField] private AILogicController _AILogicController;

    private float _currentPlacingTime;
    public void IAwake()
    {
    }

    public void IStart()
    {
        _availableEnemyBuildingGrid = new List<BuildingGrid>(_enemyBuildingGrid);
    }

    public void ResetActionValue()
    {
        _currentPlacingTime = 0;
    }

    public override void PlacingCardLogic(Action OnTurnFinished)
    {
        switch (CurrentCardState)
        {
            case PlacingCardState.Start:
                ResetActionValue();
                SwitchCardState(PlacingCardState.Placing);
                break;
            case PlacingCardState.Placing:
                PlaceingCardState(() => SwitchCardState(PlacingCardState.End));
                break;
            case PlacingCardState.End:
                SwitchCardState(PlacingCardState.Start);
                OnTurnFinished?.Invoke();
                break;
        }
    }

    protected override void PlaceingCardState(Action OnPlacingActionFinished)
    {
        int randomPlacingTime = UnityEngine.Random.Range(2, 5);
        if (_currentPlacingTime < randomPlacingTime)
        {
            _currentPlacingTime += Time.deltaTime;
        }
        else {
            if (CardRecord.CurrentFlagNumber < _basicGameRuleSO.FlagMaximumNumber)
            {
                HandleEnemyFlagLogic();
                OnPlacingActionFinished?.Invoke();
            }
            else
            {
                HandleDefStyleAILogic();
                OnPlacingActionFinished?.Invoke();
            }
        }
    }

    private void HandleEnemyFlagLogic()
    {
        PlaceFlag();
        CardRecord.CurrentFlagNumber++;

        switch (CardRecord.CurrentFlagNumber)
        {
            case 1:
                SetCardInGridPosition(15);
                break;
            case 2:
                SetCardInGridPosition(20);
                break;
            case 3:
                SetCardInGridPosition(25);
                break;
        }


        CurrentCreatedObject.CardObject = null;
    }

    private void HandleDefStyleAILogic()
    {
        int randomDecsion = UnityEngine.Random.Range(0, 100);
        if (randomDecsion >= 30)
        {
            if (CardRecord.CurrentShieldNumber < _basicGameRuleSO.ShieldMaximumNumber)
            {
                PlaceShield(_shieldController.ShieldSpriteRenders[CardRecord.CurrentShieldNumber]);
                CardRecord.CurrentShieldNumber++;
            }
            else
            {
                //Place Cannon or attack
            }
        }
        else if (randomDecsion < 30)
        {
            if (CardRecord.CurrentCannonNumber < _basicGameRuleSO.CannonMaximumNumber)
            {
                int randomPosition = UnityEngine.Random.Range(0, _availableEnemyBuildingGrid.Count);
                if (!_availableEnemyBuildingGrid[randomPosition].enabled)
                {
                    return;
                }
                else
                {
                    PlaceCannon();
                    SetCardInGridPosition(randomPosition);
                    CardRecord.CurrentCannonNumber++;
                }
            }
            else
            {
                //Place Shield or attack
            }
        }

        CurrentCreatedObject.CardObject = null;
    }

    private void SetCardInGridPosition(int positionIndex)
    {
        if (CurrentCreatedObject.CardObject != null)
        {
            CurrentTile = _enemyBuildingGrid[positionIndex];
            CurrentCreatedObject.CardObject.transform.position = CurrentTile.GetTileCenterPosition();
            _availableEnemyBuildingGrid[positionIndex].enabled = false;
        }
    }

}
