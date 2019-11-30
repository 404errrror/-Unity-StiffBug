/*
 * 2019.11.10
 * 조이스틱입니다.
 * 조작이 이루어지면 InputManager의 데이터를 업데이트합니다.
 */

using UnityEngine;
using UnityEngine.EventSystems;

public enum EJoyStickControl
{
    JoyStickControl_Left = 0,
    JoyStickControl_Right
}

public class MJoyStick : MonoBehaviour
{
    public GameObject stick;
    public EJoyStickControl joyStickControl;

    void Start()
    {
        EventTrigger eventTrigger = GetComponent<EventTrigger>();

        /* 조작 이벤트 등록 */
        if (eventTrigger != null)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((data) => { UpdateVector(((PointerEventData)data).position); });
            eventTrigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Drag;
            entry.callback.AddListener((data) => { UpdateVector(((PointerEventData)data).position); });
            eventTrigger.triggers.Add(entry);
        }
        else
            Debug.LogError("MJoyStick.Start(), EventTrigger is null!!");

        if (stick == null)
            Debug.LogError("MJoyStick.Start(), Stick is null!!");
    }

    public void UpdateVector(Vector2 position)
    {
        /* Stick 위치 업데이트 */
        stick.transform.position = position;

        /* InputManager 데이터 업데이트 */
        switch (joyStickControl)
        {
            case EJoyStickControl.JoyStickControl_Left:    InputProperities.Instance.SetRotationL(stick.transform.localPosition); break;
            case EJoyStickControl.JoyStickControl_Right: InputProperities.Instance.SetRotationR(stick.transform.localPosition); break;
        }
    }
}
