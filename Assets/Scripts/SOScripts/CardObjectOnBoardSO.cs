using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CardPlacementObject
{
    public EnumDefs.Card CardType;
    public GameObject CardObject;
}

[CreateAssetMenu(fileName = "CardData", menuName = "Card/Card Object List", order = 1)]
public class CardObjectOnBoardSO : ScriptableObject
{
   

    public List<CardPlacementObject> cardObjects = new List<CardPlacementObject>();

    public GameObject GetCardObjectByType(EnumDefs.Card cardType)
    {
        for (int i = 0; i < cardObjects.Count; i++)
        {
            var singleCardObject = cardObjects[i];
            if (cardType.Equals(singleCardObject.CardType))
            {
                return singleCardObject.CardObject;
            }
        }
        return null;
    }
}
