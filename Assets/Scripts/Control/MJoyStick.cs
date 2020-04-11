/*
 * 2019.11.10
 * 조이스틱입니다.
 * 조작이 이루어지면 InputManager의 데이터를 업데이트합니다.
 */

using UnityEngine;
using UnityEngine.EventSystems;

public class MJoyStick : MonoBehaviour
{
    public GameObject stick;
    public bool isTouch;
    void Start()
    {
        EventTrigger eventTrigger = GetComponent<EventTrigger>();

        /* 조작 이벤트 등록 */
        if (eventTrigger != null)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((data) => {
                UpdateStickPosition(((PointerEventData)data).position);
                isTouch = true;
            });
            eventTrigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Drag;
            entry.callback.AddListener((data) => { UpdateStickPosition(((PointerEventData)data).position); });
            eventTrigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerExit;
            entry.callback.AddListener((data) => {
                isTouch = false;
            });
            eventTrigger.triggers.Add(entry);
        }
        else
            Debug.LogError("MJoyStick.Start(), EventTrigger is null!!");

        if (stick == null)
            Debug.LogError("MJoyStick.Start(), Stick is null!!");
    }

    private void UpdateStickPosition(Vector2 position)
    {
        /* Stick 위치 업데이트 */
        stick.transform.position = position;
    }


    public Vector2 GetStickLocalPosition()
    {
        return stick.transform.localPosition;
    }
}
