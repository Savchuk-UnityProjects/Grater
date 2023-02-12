using UnityEngine;
using System;

public abstract class InvokingEventWithCondition : MonoBehaviour
{
    public event Action Event;

    protected bool Condition;

    protected void Update()
    {
        if(Condition)
        {
            Event?.Invoke();
        }
    }

    private void OnDestroy()
    {
        Event = null;
    }
}