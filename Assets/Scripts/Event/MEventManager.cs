using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MEventManager : MonoSingleton<MEventManager>
{
    public delegate void EventHandlerNoParam();

    event EventHandlerNoParam ActionButton;
    bool lockActionButton;

    void Start()
    {
        lockActionButton = false;
    }

    public void Invoke_ActionButton()
    {
        if (ActionButton != null && lockActionButton == false)
        {
            ActionButton.Invoke();
        }
    }

    public void AddActionButtonEvent(EventHandlerNoParam eventHandler)
    {
        ActionButton += eventHandler;
    }

    public void RemoveActionButtonEvent(EventHandlerNoParam eventHandler)
    {
        ActionButton -= eventHandler;
    }

    public void LockActionButton()
    {
        lockActionButton = true;
    }

    public void UnLockActionButton()
    {
        lockActionButton = false;
    }
}
