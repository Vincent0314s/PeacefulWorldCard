using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Card;

public class PlayerCardPlacementController : BasicCardPlacementController, IInitialization
{
    [Header("Placement")]
    [SerializeField] private PlayerCardController _playerCardController;
    private CardPlacementObject currentCreatedObject;
    public LayerMask buildingTileMask;
    private Camera _mainCam;
    private BuildingGrid _previousTitle;
    [Header("Events")]
    [SerializeField] private GameEventSO _onFlagPlacedEvent;
    [SerializeField] private GameEventSO _onShieldPlacedEvent;
    [SerializeField] private GameEventSO _onCannonPlacedEvent;
    [SerializeField] private SpriteRenderer[] _shieldSpriteRenderers;
    [SerializeField] private Color _ghostColor;
    private bool _isHoldingCard;

    public void IAwake()
    {
        _mainCam = Camera.main;
    }

    public void IStart()
    {
        //Get Buttom from Name or Enum instead of number.
        _playerCardController.SubscribeButtonDownEvent(0, () => CreateCardObject(EnumDefs.Card.Flag));
        _playerCardController.SubscribeButtonDownEvent(1, () => PlaceShield());
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

    public void PlayerPlaceingCardAction()
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
                }
            }
        }
    }

    private void CheckWhichCardisBeingPlaced(EnumDefs.Card cardType)
    {
        switch (cardType)
        {
            case EnumDefs.Card.Flag:
                if (PlayerCardRecord.CurrentFlagNumber < _basicGameRuleSO.FlagMaximumNumber)
                {
                    PlayerCardRecord.CurrentFlagNumber++;
                    if (PlayerCardRecord.CurrentFlagNumber >= _basicGameRuleSO.FlagMaximumNumber)
                    {
                        _playerCardController.EnableCardPlacementButton(0, false);
                    }
                }
                _onFlagPlacedEvent.RaiseEvent();
                break;
            case EnumDefs.Card.Shield:
                if (PlayerCardRecord.CurrentShieldNumber < _basicGameRuleSO.ShieldMaximumNumber)
                {
                    PlayerCardRecord.CurrentShieldNumber++;
                    if (PlayerCardRecord.CurrentShieldNumber >= _basicGameRuleSO.ShieldMaximumNumber)
                    {
                        _playerCardController.EnableCardPlacementButton(1, false);
                    }
                }
                _onShieldPlacedEvent.RaiseEvent();
                break;
            case EnumDefs.Card.Cannon:
                if (PlayerCardRecord.CurrentCannonNumber < _basicGameRuleSO.CannonMaximumNumber)
                {
                    PlayerCardRecord.CurrentCannonNumber++;
                    if (PlayerCardRecord.CurrentCannonNumber >= _basicGameRuleSO.CannonMaximumNumber)
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
