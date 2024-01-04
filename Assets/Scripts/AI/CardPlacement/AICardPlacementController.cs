using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Card;

public class AICardPlacementController : BasicCardPlacementController, IInitialization
{
    [Header("Shield")]
    [SerializeField] private SpriteRenderer[] _enemyShieldSpriteRenderers;
    [SerializeField] private BuildingGrid[] _enemyBuildingGrid;
    private List<BuildingGrid> _availableEnemyBuildingGrid;
    private CardPlacementObject currentEnemyCreatedObject;
    private BuildingGrid _currentEnemyTile;

    [SerializeField] private AILogicController _AILogicController;

    public void IAwake()
    {
    }

    public void IStart()
    {
        _availableEnemyBuildingGrid = new List<BuildingGrid>(_enemyBuildingGrid);
    }

    public void AIPlacingCardAction()
    {
        //_playerCardController.EnableCardPlacementButton(0, false);
        //_playerCardController.EnableCardPlacementButton(1, false);
        //_playerCardController.EnableCardPlacementButton(2, false);
        if (EnemyCardRecord.CurrentFlagNumber < _basicGameRuleSO.FlagMaximumNumber)
        {
            HandleEnemyFlagLogic();
        }
        else
        {
            HandleDefStyleAILogic();
        }
    }

    private void HandleEnemyFlagLogic()
    {
        switch (EnemyCardRecord.CurrentFlagNumber)
        {
            case 0:
                if (currentEnemyCreatedObject.CardObject == null)
                {
                    currentEnemyCreatedObject.CardObject = Instantiate(_cardObjectSO.GetCardObjectByType(EnumDefs.Card.Flag));
                }
                SetCardInGridPosition(15);
                EnemyCardRecord.CurrentFlagNumber++;
                _availableEnemyBuildingGrid[15].enabled = false;
                break;
            case 1:
                if (currentEnemyCreatedObject.CardObject == null)
                {
                    currentEnemyCreatedObject.CardObject = Instantiate(_cardObjectSO.GetCardObjectByType(EnumDefs.Card.Flag));
                }
                SetCardInGridPosition(20);
                EnemyCardRecord.CurrentFlagNumber++;
                _availableEnemyBuildingGrid[15].enabled = false;
                break;
            case 2:
                if (currentEnemyCreatedObject.CardObject == null)
                {
                    currentEnemyCreatedObject.CardObject = Instantiate(_cardObjectSO.GetCardObjectByType(EnumDefs.Card.Flag));
                }
                SetCardInGridPosition(25);
                EnemyCardRecord.CurrentFlagNumber++;
                _availableEnemyBuildingGrid[15].enabled = false;
                break;
        }


        currentEnemyCreatedObject.CardObject = null;
    }

    private void HandleDefStyleAILogic()
    {
        int randomDecsion = Random.Range(0, 100);
        if (randomDecsion >= 30)
        {
            if (EnemyCardRecord.CurrentShieldNumber < _basicGameRuleSO.ShieldMaximumNumber)
            {
                PlaceEnemyShield();
            }
            else
            {
                //Do other Logic
            }
        }
        else if (randomDecsion < 30)
        {
            if (EnemyCardRecord.CurrentCannonNumber < _basicGameRuleSO.CannonMaximumNumber)
            {
                if (currentEnemyCreatedObject.CardObject == null)
                {
                    currentEnemyCreatedObject.CardObject = Instantiate(_cardObjectSO.GetCardObjectByType(EnumDefs.Card.Cannon));
                }
                int randomPosition = Random.Range(0, _availableEnemyBuildingGrid.Count);
                if (!_availableEnemyBuildingGrid[randomPosition].enabled)
                {
                    return;
                }
                else
                {
                    SetCardInGridPosition(randomPosition);
                    _availableEnemyBuildingGrid[randomPosition].enabled = false;
                    EnemyCardRecord.CurrentCannonNumber++;
                }
            }
            else
            {
                //Do other Logic
            }
        }

        currentEnemyCreatedObject.CardObject = null;
    }

    private void SetCardInGridPosition(int positionIndex)
    {
        if (currentEnemyCreatedObject.CardObject != null)
        {
            _currentEnemyTile = _enemyBuildingGrid[positionIndex];
            currentEnemyCreatedObject.CardObject.transform.position = _currentEnemyTile.GetTileCenterPosition();
        }
    }

}
