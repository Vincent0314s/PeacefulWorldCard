using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(EventTrigger))]
public class CustomButton : MonoBehaviour
{
    private EventTrigger _eventTrigger;

    private EventTrigger.Entry OnEnterEvent;
    private EventTrigger.Entry OnDownEvent;
    private EventTrigger.Entry OnExitEvent;
    private EventTrigger.Entry OnClickEvent;

    private void Awake()
    {
        _eventTrigger = GetComponent<EventTrigger>();
        _eventTrigger.triggers.Clear();
    }

    private void Start()
    {
        OnEnterEvent = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        OnDownEvent = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerDown
        };
        OnExitEvent = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerExit
        };
        OnClickEvent = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };
        _eventTrigger.triggers.Add(OnEnterEvent);
        _eventTrigger.triggers.Add(OnDownEvent);
        _eventTrigger.triggers.Add(OnExitEvent);
        _eventTrigger.triggers.Add(OnClickEvent);
    }

    public void EnableEventTrigger(bool enableTrigger)
    {
        _eventTrigger.enabled = enableTrigger;
    }

    public void AddButtonEnterEvent(Action eventCallback)
    {
        OnEnterEvent.callback.AddListener(delegate
        {
            eventCallback?.Invoke();
        });
    }

    public void AddButtonDownEvent(Action eventCallback)
    {
        OnDownEvent.callback.AddListener(delegate
        {
            eventCallback?.Invoke();
        });
    }

    public void AddButtonExitEvent(Action eventCallback)
    {
        OnExitEvent.callback.AddListener(delegate
        {
            eventCallback?.Invoke();
        });
    }

    public void AddButtonClickEvent(Action eventCallback)
    {
        OnClickEvent.callback.AddListener(delegate
        {
            eventCallback?.Invoke();
        });
    }

    public void ClearButtonEvent()
    {
        _eventTrigger.triggers.Clear();
    }
}
