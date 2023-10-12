using System;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public GameEvent Event;
    public Action<object> EventResponse;
    public UnityEvent UnityEventResponse;

    private void OnValidate()
    {
        if(Event != null)
            gameObject.name = Event.name;
    }

    public void OnEnable()
    {
        Event.RegisterListener(this);
    }

    public void OnDisable()
    {
        Event.UnRegisterListener(this);
        EventResponse = null;
    }

    public void OnUnityEventRaised() {
        UnityEventResponse?.Invoke();
    }

    public void OnEventRaised(object data)
    {
        EventResponse?.Invoke(data);
    }
}
