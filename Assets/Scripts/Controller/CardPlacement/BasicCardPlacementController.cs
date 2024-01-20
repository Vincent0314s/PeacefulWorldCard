using UnityEngine;
using Core.Card;
using System;

public class BasicCardPlacementController : MonoBehaviour
{
    [System.Serializable]
    public struct CardRecordOnBoard
    {
        public int CurrentFlagNumber;
        public int CurrentShieldNumber;
        public int CurrentCannonNumber;
    }

    public enum PlacingCardState
    {
        Start,
        Placing,
        End
    }

    //Common
    [SerializeField] protected CardObjectOnBoardSO CardObjectSO;
    protected BuildingGrid CurrentTile;
    protected CardPlacementObject CurrentCreatedObject;

    [SerializeField] protected GameBoardRuleSO _basicGameRuleSO;
    [Header("Card On Boards")]
    [SerializeField] protected Color _solidColor;
    [SerializeField] protected CardRecordOnBoard CardRecord;

    [SerializeField] protected PlacingCardState CurrentCardState;

    protected virtual void PlaceShield(SpriteRenderer currentShield)
    {
        currentShield.enabled = true;
        currentShield.color = _solidColor;
    }

    protected virtual void PlaceFlag()
    {
        if (CurrentCreatedObject.CardObject != null)
        {
            return;
        }
        CurrentCreatedObject.CardObject = Instantiate(CardObjectSO.GetCardObjectByType(EnumDefs.Card.Flag));
        CurrentCreatedObject.CardType = EnumDefs.Card.Flag;
    }

    protected virtual void PlaceCannon()
    {
        if (CurrentCreatedObject.CardObject != null)
        {
            return;
        }

        CurrentCreatedObject.CardObject = Instantiate(CardObjectSO.GetCardObjectByType(EnumDefs.Card.Cannon));
        CurrentCreatedObject.CardType = EnumDefs.Card.Cannon;
    }

    public virtual void PlacingCardLogic(Action OnTurnFinished)
    {
    }

    protected virtual void PlaceingCardState(Action OnPlacingActionFinished)
    {
    }

    protected void SwitchCardState(PlacingCardState newState)
    {
        CurrentCardState = newState;
    }
}
