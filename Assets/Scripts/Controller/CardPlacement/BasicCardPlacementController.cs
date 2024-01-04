using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Card;

public class BasicCardPlacementController : MonoBehaviour
{
    [System.Serializable]
    public struct CardRecordOnBoard
    {
        public int CurrentFlagNumber;
        public int CurrentShieldNumber;
        public int CurrentCannonNumber;
    }

    //Common
    [SerializeField] protected CardObjectOnBoardSO _cardObjectSO;
    protected BuildingGrid _currentTile;
    [SerializeField] protected GameBoardRuleSO _basicGameRuleSO;
    [Header("Card On Boards")]
    [SerializeField] protected CardRecordOnBoard PlayerCardRecord;
    [SerializeField] protected CardRecordOnBoard EnemyCardRecord;
    [SerializeField] protected Color _solidColor;

    public void PlaceShield()
    {
        //SpriteRenderer currentShield = _shieldSpriteRenderers[PlayerCardRecord.CurrentShieldNumber];
        //currentShield.enabled = true;
        //currentShield.color = _solidColor;
        //CheckWhichCardisBeingPlaced(EnumDefs.Card.Shield);
    }

    public void PlaceEnemyShield()
    {
        //SpriteRenderer currentShield = _enemyShieldSpriteRenderers[EnemyCardRecord.CurrentShieldNumber];
        //currentShield.enabled = true;
        //currentShield.color = _solidColor;
        //EnemyCardRecord.CurrentShieldNumber++;
    }
}
