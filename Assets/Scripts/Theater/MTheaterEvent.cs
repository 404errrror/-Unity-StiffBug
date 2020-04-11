using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Febucci.UI;

public class MTheaterEvent : MonoBehaviour
{
    public TextAnimatorPlayer textAnimatorPlayer;

    void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(textAnimatorPlayer, $"Text Animator Player component is null in {gameObject.name}");
        textAnimatorPlayer.textAnimator.onEvent += OnEvent;
    }

    void OnEvent(string message)
    {
        switch (message)
        {
            case "lockactionbutton":
                MEventManager.Instance.LockActionButton();
                break;
            case "unlockactionbutton":
                MEventManager.Instance.UnLockActionButton();
                break;
        }
    }
}
