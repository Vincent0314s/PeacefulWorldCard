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

    [SerializeField] protected CardObjectOnBoardSO CardObjectSO;
    [SerializeField] protected ShieldController _shieldController;

    protected BuildingGrid CurrentTile;
    protected CardPlacementObject CurrentCreatedObject;

    [SerializeField] protected GameBoardRuleSO _basicGameRuleSO;
    [Header("Card On Boards")]
    [SerializeField] protected Color _solidColor;
    [SerializeField] protected CardRecordOnBoard CardRecord;

    [SerializeField] protected PlacingCardState CurrentCardState;

    protected virtual void PlaceShield(Card_Shield currentShield)
    {
        currentShield.ShieldEnable(true);
        currentShield.SetColor(_solidColor);
        currentShield.SetCardOwner(this);
    }

    protected virtual void PlaceFlag()
    {
        if (CurrentCreatedObject.CardObject != null)
        {
            return;
        }
        CurrentCreatedObject.CardObject = Instantiate(CardObjectSO.GetCardObjectByType(EnumDefs.Card.Flag));
        CurrentCreatedObject.CardObject.GetComponent<CardBase>().SetCardOwner(this);
        CurrentCreatedObject.CardType = EnumDefs.Card.Flag;
    }

    protected virtual void PlaceCannon()
    {
        if (CurrentCreatedObject.CardObject != null)
        {
            return;
        }

        CurrentCreatedObject.CardObject = Instantiate(CardObjectSO.GetCardObjectByType(EnumDefs.Card.Cannon));
        CurrentCreatedObject.CardObject.GetComponent<CardBase>().SetCardOwner(this);
        CurrentCreatedObject.CardType = EnumDefs.Card.Cannon;
    }

    public void DestroyCard(EnumDefs.Card cardType)
    {
        switch (cardType) {
            case EnumDefs.Card.Flag:
                CardRecord.CurrentFlagNumber--;
                break;
            case EnumDefs.Card.Cannon:
                CardRecord.CurrentCannonNumber--;
                break;
            case EnumDefs.Card.Shield:
                CardRecord.CurrentShieldNumber--;
                break;
        }
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
