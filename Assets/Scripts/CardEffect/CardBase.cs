using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBase : MonoBehaviour
{
    protected BasicCardPlacementController CardPlacementController;
    public void SetCardOwner(BasicCardPlacementController basicCardPlacementController)
    {
        CardPlacementController = basicCardPlacementController;
    }
}
