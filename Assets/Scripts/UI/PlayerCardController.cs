using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerCardController : MonoBehaviour
{
    [SerializeField] private CustomButton[] playerCards;

    public void SubscribeButtonDownEvent(int buttonIndex, Action buttonEvent)
    {
        playerCards[buttonIndex].AddButtonDownEvent(buttonEvent);
    }

    public void UnSubscribeAllButtonEvents()
    {
        foreach (var button in playerCards)
        {
            button.ClearButtonEvent();
        }
    }

    public void EnableCardPlacementButton(int buttonIndex, bool enabledCard)
    {
        playerCards[buttonIndex].GetComponent<Button>().interactable = enabledCard;
    }
}
