using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Card;
using System;

public class PlayerCardPlacementController : BasicCardPlacementController, IInitialization
{
    [Header("Placement")]
    [SerializeField] private PlayerCardController _playerCardController;
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

    private bool _hasPlacedShield;
    private bool _flagButtonEnabled;
    private bool _shieldButtonEnabled;
    private bool _cannonButtonEnabled;


    public void IAwake()
    {
        _mainCam = Camera.main;
    }

    public void IStart()
    {
        //Get Buttom from Name or Enum instead of number.
        _playerCardController.SubscribeButtonDownEvent(0, () => PlaceFlag());
        _playerCardController.SubscribeButtonDownEvent(1, () => PlaceShield(_shieldSpriteRenderers[CardRecord.CurrentShieldNumber]));
        _playerCardController.SubscribeButtonDownEvent(2, () => PlaceCannon());

        //Eveey battle start
        EnableAllCardPlacementButton(false);
    }

    protected override void PlaceFlag()
    {
        base.PlaceFlag();
        _isHoldingCard = true;
    }

    protected override void PlaceCannon()
    {
        base.PlaceCannon();
        _isHoldingCard = true;
    }

    protected override void PlaceShield(SpriteRenderer currentShield)
    {
        base.PlaceShield(currentShield);
        _hasPlacedShield = true;
    }

    public override void PlacingCardLogic(Action OnTurnFinished)
    {
        switch (CurrentCardState)
        {
            case PlacingCardState.Start:
                EnableAllCardPlacementButton(true);
                SwitchCardState(PlacingCardState.Placing);
                break;
            case PlacingCardState.Placing:
                PlaceingCardState(() => SwitchCardState(PlacingCardState.End));
                break;
            case PlacingCardState.End:
                EnableAllCardPlacementButton(false);
                SwitchCardState(PlacingCardState.Start);
                OnTurnFinished?.Invoke();
                break;
        }
    }

    protected override void PlaceingCardState(Action OnPlacingActionFinished)
    {
        if (_isHoldingCard)
        {
            Vector3 mouse = Input.mousePosition;
            Ray castPoint = _mainCam.ScreenPointToRay(mouse);
            RaycastHit hit;

            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, buildingTileMask))
            {
                CurrentCreatedObject.CardObject.transform.position = hit.point;
                BuildingGrid tile = hit.transform.GetComponent<BuildingGrid>();
                if (CurrentTile != null)
                {
                    _previousTitle = CurrentTile;
                    _previousTitle.DeSelected();
                }

                CurrentTile = tile;
                CurrentTile.Selected();
                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    AddCardToRecrod(CurrentCreatedObject.CardType);
                    PlaceingCard();
                    OnPlacingActionFinished?.Invoke();
                }
            }
        }
        else if (_hasPlacedShield)
        {
            AddCardToRecrod(EnumDefs.Card.Shield);
            _hasPlacedShield = false;
            OnPlacingActionFinished?.Invoke();
        }
    }

    public void EnableAllCardPlacementButton(bool isEnabled)
    {
        if (isEnabled)
        {
            CheckCardCondition();
            _playerCardController.EnableCardPlacementButton(0, _flagButtonEnabled);
            _playerCardController.EnableCardPlacementButton(1, _shieldButtonEnabled);
            _playerCardController.EnableCardPlacementButton(2, _cannonButtonEnabled);
        }
        else
        {
            _playerCardController.EnableCardPlacementButton(0, false);
            _playerCardController.EnableCardPlacementButton(1, false);
            _playerCardController.EnableCardPlacementButton(2, false);
        }
    }

    private void HighlighShield()
    {

    }

    private void PlaceingCard()
    {
        CurrentCreatedObject.CardObject.transform.position = CurrentTile.GetTileCenterPosition();
        CurrentTile.HasCard();
        CurrentTile = null;
        CurrentCreatedObject.CardObject = null;
        _isHoldingCard = false;
    }

    private void CheckCardCondition()
    {
        _flagButtonEnabled = (CardRecord.CurrentFlagNumber >= _basicGameRuleSO.FlagMaximumNumber) ? false : true;
        _shieldButtonEnabled = (CardRecord.CurrentShieldNumber >= _basicGameRuleSO.ShieldMaximumNumber) ? false : true;
        _cannonButtonEnabled = (CardRecord.CurrentCannonNumber >= _basicGameRuleSO.CannonMaximumNumber) ? false : true;
    }

    private void AddCardToRecrod(EnumDefs.Card cardType)
    {
        switch (cardType)
        {
            case EnumDefs.Card.Flag:
                if (CardRecord.CurrentFlagNumber < _basicGameRuleSO.FlagMaximumNumber)
                {
                    CardRecord.CurrentFlagNumber++;
                }
                _onFlagPlacedEvent.RaiseEvent();
                break;
            case EnumDefs.Card.Shield:
                if (CardRecord.CurrentShieldNumber < _basicGameRuleSO.ShieldMaximumNumber)
                {
                    CardRecord.CurrentShieldNumber++;
                }
                _onShieldPlacedEvent.RaiseEvent();
                break;
            case EnumDefs.Card.Cannon:
                if (CardRecord.CurrentCannonNumber < _basicGameRuleSO.CannonMaximumNumber)
                {
                    CardRecord.CurrentCannonNumber++;
                }
                _onCannonPlacedEvent.RaiseEvent();
                break;
        }
        //_playerCardController.EnableCardPlacementButton(0, _flagButtonEnabled);
        //_playerCardController.EnableCardPlacementButton(1, _shieldButtonEnabled);
        //_playerCardController.EnableCardPlacementButton(2, _cannonButtonEnabled);
    }

    private void OnDisable()
    {
        _playerCardController.UnSubscribeAllButtonEvents();
    }
}
