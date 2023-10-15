using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Game Event", menuName = "Event/UnityEvent", order = 2)]
public class GameEventSO : ScriptableObject
{
    public string EventName;
    public UnityEvent Event;

    public void RaiseEvent()
    {
        Event?.Invoke();
    }
}
