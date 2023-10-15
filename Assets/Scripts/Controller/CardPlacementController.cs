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

    private bool _isHoldingCard;

    private void Awake()
    {
        _mainCam = Camera.main;
    }

    private void Start()
    {
        //Get Buttom from Name or Enum instead of number.
        _playerCardController.SubscribeButtonDownEvent(0, () => CreateCardObject(EnumDefs.Card.Flag));
        _playerCardController.SubscribeButtonDownEvent(1, () => CreateCardObject(EnumDefs.Card.Shield));
        _playerCardController.SubscribeButtonDownEvent(2, () => CreateCardObject(EnumDefs.Card.Cannon));
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
                }
            }
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
}
