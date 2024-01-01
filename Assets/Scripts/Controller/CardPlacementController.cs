using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Card;

public class CardPlacementController : MonoBehaviour
{
    [System.Serializable]
    public struct CardRecordOnBoard
    {
        public int CurrentFlagNumber;
        public int CurrentShieldNumber;
        public int CurrentCannonNumber;
    }

    //Change to reference from GameMB
    [Header("Placement")]
    [SerializeField] private PlayerCardController _playerCardController;

    [SerializeField] private CardObjectOnBoardSO _cardObjectSO;
    private CardPlacementObject currentCreatedObject;
    public LayerMask buildingTileMask;

    private BuildingGrid _currentTile;
    private BuildingGrid _previousTitle;
    private Camera _mainCam;


    //Put this to Rule SO?
    public const int FlagMaximumNumber = 3;
    public const int ShieldMaximumNumber = 5;
    public const int CannonMaximumNumber = 3;

    [Header("Card On Boards")]
    public CardRecordOnBoard PlayerCardRecord;
    public CardRecordOnBoard EnemyCardRecord;


    [Header("Events")]
    [SerializeField] private GameEventSO _onFlagPlacedEvent;
    [SerializeField] private GameEventSO _onShieldPlacedEvent;
    [SerializeField] private GameEventSO _onCannonPlacedEvent;

    [Header("Shield")]
    [SerializeField] private SpriteRenderer[] _shieldSpriteRenderers;
    [SerializeField] private SpriteRenderer[] _enemyShieldSpriteRenderers;
    [SerializeField] private Color _ghostColor;
    [SerializeField] private Color _solidColor;

    [SerializeField] private GameBoardStateController _gameStateController;
    [SerializeField] private BuildingGrid[] _enemyBuildingGrid;
    private List<BuildingGrid> _availableEnemyBuildingGrid;

    private CardPlacementObject currentEnemyCreatedObject;
    private BuildingGrid _currentEnemyTile;

    private bool _isHoldingCard;

    private void Awake()
    {
        _mainCam = Camera.main;
    }

    private void Start()
    {
        //Get Buttom from Name or Enum instead of number.
        _playerCardController.SubscribeButtonDownEvent(0, () => CreateCardObject(EnumDefs.Card.Flag));
        _playerCardController.SubscribeButtonDownEvent(1, () => PlaceShield());
        _playerCardController.SubscribeButtonDownEvent(2, () => CreateCardObject(EnumDefs.Card.Cannon));

        _availableEnemyBuildingGrid = new List<BuildingGrid>(_enemyBuildingGrid);
    }

    public void CreateCardObject(EnumDefs.Card cardType)
    {
        if (currentCreatedObject.CardObject == null)
        {
            currentCreatedObject.CardObject = Instantiate(_cardObjectSO.GetCardObjectByType(cardType));
            currentCreatedObject.CardType = cardType;
            _isHoldingCard = true;
        }
    }

    private void Update()
    {
        if (_gameStateController.turnOrder == GameBoardStateController.TurnOrder.Player)
        {
            _playerCardController.EnableCardPlacementButton(0, true);
            _playerCardController.EnableCardPlacementButton(1, true);
            _playerCardController.EnableCardPlacementButton(2, true);
            if (_isHoldingCard)
            {
                Vector3 mouse = Input.mousePosition;
                Ray castPoint = _mainCam.ScreenPointToRay(mouse);
                RaycastHit hit;

                if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, buildingTileMask))
                {
                    currentCreatedObject.CardObject.transform.position = hit.point;
                    BuildingGrid tile = hit.transform.GetComponent<BuildingGrid>();
                    if (_currentTile != null)
                    {
                        _previousTitle = _currentTile;
                        _previousTitle.DeSelected();
                    }

                    _currentTile = tile;
                    _currentTile.Selected();
                    if (Input.GetKeyUp(KeyCode.Mouse0))
                    {
                        CheckWhichCardisBeingPlaced(currentCreatedObject.CardType);
                        //Put this in a method.
                        currentCreatedObject.CardObject.transform.position = _currentTile.GetTileCenterPosition();
                        _currentTile.PlaceCard();
                        _currentTile = null;
                        currentCreatedObject.CardObject = null;
                        _isHoldingCard = false;
                        _gameStateController.SwapTurnOrder();
                    }
                }
            }
        }
        else if (_gameStateController.turnOrder == GameBoardStateController.TurnOrder.Enemy)
        {
            _playerCardController.EnableCardPlacementButton(0, false);
            _playerCardController.EnableCardPlacementButton(1, false);
            _playerCardController.EnableCardPlacementButton(2, false);
            if (EnemyCardRecord.CurrentFlagNumber < FlagMaximumNumber)
            {
                HandleEnemyFlagLogic();
            }
            else
            {
                HandleDefStyleAILogic();
            }
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
        _gameStateController.SwapTurnOrder();
    }

    private void HandleDefStyleAILogic()
    {
        int randomDecsion = Random.Range(0, 100);
        if (randomDecsion >= 30)
        {
            if (EnemyCardRecord.CurrentShieldNumber < ShieldMaximumNumber)
            {
                PlaceEnemyShield();
                _gameStateController.SwapTurnOrder();
            }
            else
            {
                //Do other Logic
            }
        }
        else if (randomDecsion < 30)
        {
            if (EnemyCardRecord.CurrentCannonNumber < CannonMaximumNumber)
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
                    _gameStateController.SwapTurnOrder();
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

    private void CheckWhichCardisBeingPlaced(EnumDefs.Card cardType)
    {
        switch (cardType)
        {
            case EnumDefs.Card.Flag:
                if (PlayerCardRecord.CurrentFlagNumber < FlagMaximumNumber)
                {
                    PlayerCardRecord.CurrentFlagNumber++;
                    if (PlayerCardRecord.CurrentFlagNumber >= FlagMaximumNumber)
                    {
                        _playerCardController.EnableCardPlacementButton(0, false);
                    }
                }
                _onFlagPlacedEvent.RaiseEvent();
                break;
            case EnumDefs.Card.Shield:
                if (PlayerCardRecord.CurrentShieldNumber < ShieldMaximumNumber)
                {
                    PlayerCardRecord.CurrentShieldNumber++;
                    if (PlayerCardRecord.CurrentShieldNumber >= ShieldMaximumNumber)
                    {
                        _playerCardController.EnableCardPlacementButton(1, false);
                    }
                }
                _onShieldPlacedEvent.RaiseEvent();
                break;
            case EnumDefs.Card.Cannon:
                if (PlayerCardRecord.CurrentCannonNumber < CannonMaximumNumber)
                {
                    PlayerCardRecord.CurrentCannonNumber++;
                    if (PlayerCardRecord.CurrentCannonNumber >= CannonMaximumNumber)
                    {
                        _playerCardController.EnableCardPlacementButton(2, false);
                    }
                }
                _onCannonPlacedEvent.RaiseEvent();
                break;
        }
    }

    private void OnDisable()
    {
        _playerCardController.UnSubscribeAllButtonEvents();
    }

    public void PlaceShield()
    {
        SpriteRenderer currentShield = _shieldSpriteRenderers[PlayerCardRecord.CurrentShieldNumber];
        currentShield.enabled = true;
        currentShield.color = _solidColor;
        CheckWhichCardisBeingPlaced(EnumDefs.Card.Shield);
        _gameStateController.SwapTurnOrder();
    }

    public void PlaceEnemyShield()
    {
        SpriteRenderer currentShield = _enemyShieldSpriteRenderers[EnemyCardRecord.CurrentShieldNumber];
        currentShield.enabled = true;
        currentShield.color = _solidColor;
        EnemyCardRecord.CurrentShieldNumber++;
    }
}
