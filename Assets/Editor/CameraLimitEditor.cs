using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraLimit))]
public class CameraLimitEditor : Editor {
    static CameraLimit myScript;
    static CameraLimit.RectEnum nowFocus = CameraLimit.RectEnum.None;
    CameraLimit.RectEnum editInfo = CameraLimit.RectEnum.None;

    void OnEnable()
    {
        myScript = (CameraLimit)target;
        Initialize();
    }

    void Initialize()
    {
        Undo.RecordObject(myScript, "Initialize" + myScript.transform.name);
        myScript.Initialize();
    }

    public  override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Space(10);

        if (GUILayout.Button("Reset Limit Range"))
        {
            myScript.isInit = false;
            Initialize();
        }
    }

    public void OnSceneGUI()
    {
            DrawHandle();

            MouseCheck();

        if(editInfo != CameraLimit.RectEnum.None)
        {
            if (editInfo == CameraLimit.RectEnum.Left)
            {
                myScript.limitRect.left = MouseInfo.Position.x;
                if (myScript.limitRect.right < myScript.limitRect.left)
                myScript.limitRect.right = myScript.limitRect.left;
            }
            else if (editInfo == CameraLimit.RectEnum.Top)
            {
                myScript.limitRect.top = MouseInfo.Position.y;
                if (myScript.limitRect.top < myScript.limitRect.bottom)
                    myScript.limitRect.bottom = myScript.limitRect.top;
            }
            else if (editInfo == CameraLimit.RectEnum.Right)
            {
                myScript.limitRect.right = MouseInfo.Position.x;
                if (myScript.limitRect.right < myScript.limitRect.left)
                    myScript.limitRect.left = myScript.limitRect.right;
            }
            else if (editInfo == CameraLimit.RectEnum.Bottom)
            {
                myScript.limitRect.bottom = MouseInfo.Position.y;
                if (myScript.limitRect.top < myScript.limitRect.bottom)
                    myScript.limitRect.top = myScript.limitRect.bottom;
            }

        }
    }

    /// <summary>
    /// 마우스가 핸들을 눌렀는지 검사합니다.
    /// </summary>
    void MouseCheck()
    {
        Event currentEvent = Event.current;
        switch (currentEvent.rawType)
        {
            case EventType.MouseDown:
                if (nowFocus != CameraLimit.RectEnum.None && currentEvent.button == 0)
                {
                    Undo.RecordObject(target, "Fix Limit Range" + myScript.transform.name);
                    editInfo = nowFocus;
                    GUIUtility.hotControl = 0;
                    currentEvent.Use();
                }
                break;
            case EventType.MouseUp:
                if (editInfo != CameraLimit.RectEnum.None && currentEvent.button == 0)
                {
                    editInfo = CameraLimit.RectEnum.None;
                    GUIUtility.hotControl = 0;
                    currentEvent.Use();
                }
                break;
        }
    }

    /// <summary>
    /// 핸들을 그립니다.
    /// </summary>
    static void DrawHandle()
    {
        float heightMid = myScript.limitRect.height * 0.5f;
        float widthMid  = myScript.limitRect.width * 0.5f;

        // 점들을 찍어줍니다.
        Handles.CubeHandleCap(0, new Vector2(myScript.limitRect.left, heightMid),  Quaternion.identity, Camera.current.orthographicSize * 0.02f, EventType.Repaint);
        Handles.CubeHandleCap(0, new Vector2(myScript.limitRect.right, heightMid), Quaternion.identity, Camera.current.orthographicSize * 0.02f, EventType.Repaint);
        Handles.CubeHandleCap(0, new Vector2(widthMid, myScript.limitRect.top),    Quaternion.identity, Camera.current.orthographicSize * 0.02f, EventType.Repaint);
        Handles.CubeHandleCap(0, new Vector2(widthMid, myScript.limitRect.bottom), Quaternion.identity, Camera.current.orthographicSize * 0.02f, EventType.Repaint);

        // 마우스와 가까이 있는 점에는 빨간 점을 찍어줍니다.
        Handles.color = Color.red;
        Vector2 minPoint = myScript.FindMinPosition(MouseInfo.Position, out nowFocus);
        if (HandleUtility.DistanceToRectangle(minPoint, Quaternion.identity, Camera.current.orthographicSize * 0.02f * 0.5f)
           < 50)
            Handles.CubeHandleCap(0, minPoint, Quaternion.identity, Camera.current.orthographicSize * 0.02f, EventType.Repaint);
        else
            nowFocus = CameraLimit.RectEnum.None;
        Handles.color = Color.white;

        // 씬을 다시 그려줍니다.
        SceneView.lastActiveSceneView.Repaint();
    }

}
