using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Flag : CardBase, IDestroyable
{
    public void DestroyObject()
    {
        CardPlacementController.DestroyCard(EnumDefs.Card.Flag);
        Destroy(transform.gameObject);
    }
}
